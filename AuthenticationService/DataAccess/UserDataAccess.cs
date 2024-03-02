using AuthenticationService.Messages;
using Common;
using Common.DataAccess;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.DataAccess;

public class UserDataAccess : IUserDataAccess
{
    private readonly ApplicationDbContext _context;

    public UserDataAccess(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.Include(a => a.AspNetUserRoles).ToListAsync();
    }

    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await _context.Users
            .Include(a => a.AspNetUserRoles)
            .ThenInclude(a => a.AspNetRole).Include(a => a.Applications).ThenInclude(a => a.Application)
            .FirstOrDefaultAsync(a => a.NormalizedEmail == email.ToLower());
    }

    public async Task<bool> UserExists(string email)
    {
        return await _context.Users.AnyAsync(a=> a.Email == email.ToLower());
    }
}