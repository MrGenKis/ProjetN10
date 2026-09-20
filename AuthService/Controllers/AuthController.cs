using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequest request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return BadRequest(
                "Un utilisateur avec cet email existe déjà."
            );
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result =
            await _userManager.CreateAsync(
                user,
                request.Password
            );

        if (!result.Succeeded)
        {
            return BadRequest(
                result.Errors.Select(error => error.Description)
            );
        }

        return Ok(
            new
            {
                message = "Utilisateur créé avec succès."
            }
        );
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequest request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Unauthorized(
                "Email ou mot de passe incorrect."
            );
        }

        var passwordValid =
            await _userManager.CheckPasswordAsync(
                user,
                request.Password
            );

        if (!passwordValid)
        {
            return Unauthorized(
                "Email ou mot de passe incorrect."
            );
        }

        var expiration = DateTime.UtcNow.AddMinutes(
            int.Parse(
                _configuration["Jwt:ExpiresMinutes"] ?? "60"
            )
        );

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id
            ),

            new(
                ClaimTypes.Email,
                user.Email ?? string.Empty
            )
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!
            )
        );

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var jwt =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return Ok(
            new LoginResponse
            {
                Token = jwt,
                Email = user.Email ?? string.Empty,
                Expiration = expiration
            }
        );
    }
}