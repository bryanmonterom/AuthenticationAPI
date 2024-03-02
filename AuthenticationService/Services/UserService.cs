using AuthenticationService.DataAccess;
using AuthenticationService.Helpers;
using AuthenticationService.Messages;
using AuthenticationService.Messages.Request;
using Common;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Services;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    private readonly IUserDataAccess _dataAccess;

    public UserService(IUserDataAccess dataAccess, IConfiguration configuration, ILogger<UserService> _logger)
    {
        _dataAccess = dataAccess ?? throw new ArgumentNullException(nameof(dataAccess));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this._logger = _logger;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dataAccess.GetUsers();
    }

    public async Task<UserResponse> CreateUserAsync(UserRequest request)
    {
        var user = GetUserWithHash(request);
        try
        {
            if (_dataAccess.UserExists(user.NormalizedEmail))
            {
                return new UserResponse(string.Format(Constants.ErrorMessages.UserAlreadyExists, user.NormalizedEmail),
                    string.Empty, false);
            }
            await _dataAccess.CreateUserAsync(user);
            return new UserResponse(string.Empty, string.Empty, true);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            _logger.LogError(ex, Constants.ErrorMessages.ErrorSaving);
            return new UserResponse(errorMessage, string.Empty, false);
        }
    }

    public async Task<UserResponse> LoginUser(UserRequest request)
    {
        var errorMessage = string.Empty;
        var privateKey = _configuration.GetSection(Constants.Configs.AppSettingsTokenEntry).Value;
        var issuer = _configuration.GetSection(Constants.Configs.AppSettingsValidIssuer).Value;
        var audience = _configuration.GetSection(Constants.Configs.AppSettingsValidAudience).Value;

        var userInDb = await _dataAccess.GetUserAsync(request.Email);

        if (userInDb != null)
        {
            if (!TokenHelper.ValidatePassword(userInDb, request, out errorMessage))
                return new UserResponse(errorMessage, string.Empty, false);
            return new UserResponse(errorMessage, TokenHelper.CreateToken(userInDb, privateKey, issuer, audience),
                true);
        }

        return new UserResponse(Constants.ErrorMessages.UserPasswordMismatch, string.Empty, false);
    }

    private User GetUserWithHash(UserRequest request)
    {
        return new User(BCrypt.Net.BCrypt.HashPassword(request.Password), request.Email);
    }
}