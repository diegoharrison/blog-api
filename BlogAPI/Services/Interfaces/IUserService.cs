using BlogAPI.Models;

namespace BlogAPI.Services.Interfaces;

public interface IUserService
{
    Task<bool> UserExists(string username);
    Task<bool> VerifyPasswordAsync(string username, string password);
    Task CreateUserAsync(User user);
    IEnumerable<User> GetUsers();
}
