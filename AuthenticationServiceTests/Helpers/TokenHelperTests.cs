using System.Security.Claims;
using System.Text;
using AuthenticationService.Helpers;
using AuthenticationService.Messages.Request;
using Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServiceTests.Helpers;

[TestClass]
public class TokenHelperTests
{
    private readonly string privateKey = "keykeykeykeykeykeykeykeykeykeykeykeykey";

    [TestMethod]
    public void ValidatePasswordToken()
    {
        var token = TokenHelper.CreateToken(new User("Aa123456@", "user1@email.com"),
            privateKey, "bam", "audience");
        var validation =
            TokenHelper.ValidateJwtToken(token, privateKey, "bam", "audience");

        Assert.IsTrue(validation);
    }

    [TestMethod]
    [ExpectedException(typeof(SecurityTokenInvalidAudienceException))]
    public void ValidatePasswordToken1()
    {
        var token = TokenHelper.CreateToken(new User("Aa123456@", "user1@email.com"),
            privateKey, "bam", "audience");
        var validation =
            TokenHelper.ValidateJwtToken(token, privateKey, "bam", "audience1");
    }


    [TestMethod]
    [ExpectedException(typeof(SecurityTokenInvalidIssuerException))]
    public void ValidatePasswordToken2()
    {
        var token = TokenHelper.CreateToken(new User("Aa123456@", "user1@email.com"),
            privateKey, "bam1", "audience");
        var validation =
            TokenHelper.ValidateJwtToken(token, privateKey, "bam", "audience");
    }

    [TestMethod]
    public void CreateToken_ValidUser_ReturnsToken()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var issuer = "issuer";
        var audience = "audience";

        // Act
        var token = TokenHelper.CreateToken(user, privateKey, issuer, audience);

        // Assert
        Assert.IsNotNull(token);
    }


    [TestMethod]
    public void GetClaims_ValidUser_ReturnsClaims()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };

        // Act
        var claims = TokenHelper.GetClaims(user);

        // Assert
        Assert.IsNotNull(claims);
        Assert.AreEqual(1, claims.Count);
    }

    [TestMethod]
    public void GetJtw_ValidClaims_ReturnsToken()
    {
        // Arrange
        var claims = new List<Claim> { new(ClaimTypes.Email, "test@example.com") };
        var cred = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
            SecurityAlgorithms.HmacSha512Signature);
        var issuer = "issuer";
        var audience = "audience";

        // Act
        var token = TokenHelper.GetJtw(claims, cred, issuer, audience);

        // Assert
        Assert.IsNotNull(token);
    }

    [TestMethod]
    public void GetKey_ValidPrivateKey_ReturnsKey()
    {
        // Arrange

        // Act
        var key = TokenHelper.GetKey(privateKey);

        // Assert
        Assert.IsNotNull(key);
    }

    [TestMethod]
    public void ValidatePassword_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var user = new User { PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
        var request = new UserRequest { Email = "aa@emial.com", Password = "password" };
        string errorMessage;

        // Act
        var result = TokenHelper.ValidatePassword(user, request, out errorMessage);

        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
    }

    [TestMethod]
    public void ValidatePassword_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var user = new User { PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") };
        var request = new UserRequest { Email = "aa@emial.com", Password = "wrongpassword" };
        string errorMessage;

        // Act
        var result = TokenHelper.ValidatePassword(user, request, out errorMessage);

        // Assert
        Assert.IsFalse(result);
        Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
    }
}