using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication4.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Тестовый вход. Возвращает JWT для учебных запросов.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromQuery] string userName, [FromQuery] string password)
    {
        if (userName != "admin" || password != "1234")
        {
            return Unauthorized("Неверный логин или пароль");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("В appsettings.json не задан Jwt:Key");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
    }
}
