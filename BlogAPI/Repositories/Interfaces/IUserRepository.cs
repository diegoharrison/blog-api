using BlogAPI.Models;

namespace BlogAPI.Repositories;

public interface IUserRepository
{
    Task<bool> UserExists(string username);
    Task<User> GetUserByUsernameAsync(string username);
    Task CreateUserAsync(User user);
    IEnumerable<User> GetUsers();        
}
