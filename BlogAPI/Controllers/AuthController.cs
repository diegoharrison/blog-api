using BlogAPI.Models;
using BlogAPI.Services.Interfaces;
using BlogAPI.Services.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService? _userService;
    private readonly IJwtService _jwtService;

    [HttpPost("register")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Register(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await _userService.UserExists(model.Username);
        if (userExists)
        {
            return Conflict("Usuário já existe");
        }

        await _userService.CreateUserAsync(model);

        return CreatedAtAction(nameof(Login), new { username = model.Username }, "Usuário registrado com sucesso");
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Login(User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userExists = await _userService.UserExists(model.Username);
        
        if (!userExists)
        {
            return NotFound("Usuário não encontrado");
        }

        // Verificar as credenciais do usuário (senha, etc.)

        // Se as credenciais estiverem corretas, gerar token JWT
        var token = _jwtService.GenerateJwtToken(model);

        return Ok(token);
    }
}
