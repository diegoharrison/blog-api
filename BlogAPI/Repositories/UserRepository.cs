using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);        
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<User> GetUsers()
    {
        // Aqui você pode implementar a lógica para acessar os usuários do banco de dados
        // Por exemplo, consultando uma tabela de usuários
        return new List<User>
        {
            //new User { Id = 1, Name = "Usuário 1", Email = "usuario1@example.com" },
            //new User { Id = 2, Name = "Usuário 2", Email = "usuario2@example.com" }
        };
    }
}
