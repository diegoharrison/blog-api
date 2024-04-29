using BlogAPI.Models;

namespace BlogAPI.Services.Jwt;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}


