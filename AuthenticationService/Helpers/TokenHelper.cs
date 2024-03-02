using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Messages.Request;
using Common;
using Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Helpers;

public class TokenHelper
{
    public static string CreateToken(User user, string privateKey, string issuer, string audience)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));
        if (privateKey is null) throw new ArgumentNullException(nameof(privateKey));
        if (issuer is null) throw new ArgumentNullException(nameof(issuer));
        if (audience is null) throw new ArgumentNullException(nameof(audience));

        var claims = GetClaims(user);
        var key = GetKey(privateKey);
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var jtw = GetJtw(claims, cred, issuer, audience);

        return jtw;
    }

    public static bool ValidateJwtToken(string token, string privateKey, string issuer, string audience)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = GetKey(privateKey);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidIssuer = issuer,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        return validatedToken != null;
    }

    public static List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email)
        };

        var roleClaims = user.AspNetUserRoles?.Select(ur => new Claim(ClaimTypes.Role, ur?.AspNetRole?.Name!));
        var appClaims = user.Applications?.Select(ua => new Claim(ClaimTypes.System, ua?.Application?.Name!));


        if (roleClaims != null) claims.AddRange(roleClaims);
        if (appClaims != null) claims.AddRange(appClaims);

        return claims;
    }

    public static string GetJtw(List<Claim> claims, SigningCredentials cred, string issuer, string audience)
    {
        var token = new JwtSecurityToken(claims: claims, issuer: issuer, audience: audience,
            expires: DateTime.Now.AddHours(Constants.Configs.TokenExpirationInHours), signingCredentials: cred);
        var jtw = new JwtSecurityTokenHandler().WriteToken(token);
        return jtw;
    }

    public static SymmetricSecurityKey GetKey(string privateKey)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(privateKey ??
                                   throw new InvalidOperationException(Constants.ErrorMessages.NoTokenProvided)));
        return key;
    }

    public static bool ValidatePassword(User user, UserRequest request, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return true;
        errorMessage = Constants.ErrorMessages.UserPasswordMismatch;
        return false;
    }
}