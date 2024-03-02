using AuthenticationService.Messages;
using Common.Models;

namespace AuthenticationService.DataAccess;

public interface IUserDataAccess
{
    Task<IEnumerable<User>> GetUsers();
    Task CreateUserAsync(User user);
    Task<User?> GetUserAsync(string email);
    Task<bool> UserExists(string email);
}