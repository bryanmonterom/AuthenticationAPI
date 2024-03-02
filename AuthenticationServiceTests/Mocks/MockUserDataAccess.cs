using AuthenticationService.DataAccess;
using AuthenticationService.Messages;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationServiceTests.Mocks;

internal class MockUserDataAccess : IUserDataAccess
{
    public List<User> Users { get; set; } = new()
    {
        new User { Email = "aa@email.com", NormalizedEmail ="aa@email.com",PasswordHash = "$2a$11$8IB1uiBvHa5/7/3EelgLkeXNr04XACpKD.Os8/pFBARTgX1CYbCHi"},
        new User { Email = "aa@email.com", NormalizedEmail ="aa@email.com" },
        new User { Email = "aa@email.com", NormalizedEmail ="aa@email.com" },
    };

    public Task<IEnumerable<User>> GetUsers()
    {
        throw new NotImplementedException();
    }

    Task IUserDataAccess.CreateUserAsync(User user)
    {
        return Task.CompletedTask;
    }

    public Task<User?> GetUserAsync(string email)
    {
        return Task.FromResult(Users.Find(a => a.Email == email));
    }

    public bool UserExists(string email)
    {
        return Users.Any(a => a.Email == email.ToLower());
    }
}