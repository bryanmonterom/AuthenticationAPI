using AuthenticationService.Messages;
using AuthenticationService.Messages.Request;
using Common.Models;

namespace AuthenticationService.Services;

public interface IUserService
{
    Task<UserResponse> CreateUserAsync(UserRequest request);
    Task<UserResponse> LoginUser(UserRequest request);

    Task<IEnumerable<User>> GetAllAsync();
}