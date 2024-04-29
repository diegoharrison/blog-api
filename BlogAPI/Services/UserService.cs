using BlogAPI.Models;
using BlogAPI.Repositories;

namespace BlogAPI.Services.Interfaces;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> UserExists(string username)
    {
        return await _userRepository.UserExists(username);
    }

    public async Task<bool> VerifyPasswordAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        
        if (user is null)
            return false;

        // Verificar se a senha fornecida corresponde ao hash da senha armazenada
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }

    public async Task CreateUserAsync(User user)
    {
        // Gerar um hash seguro para a senha
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Armazenar o hash da senha no banco de dados junto com os outros dados do usuário
        user.Password = hashedPassword;
        await _userRepository.CreateUserAsync(user);
    }

    public IEnumerable<User> GetUsers()
    {
        return _userRepository.GetUsers();
    }
}
