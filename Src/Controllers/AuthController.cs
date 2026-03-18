using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAuthApi.Src.Data;
using SecureAuthApi.Src.Models.DTOs;
using SecureAuthApi.Src.Models.Entities;
using SecureAuthApi.Src.Services;

namespace SecureAuthApi.Src.Controllers;

using Microsoft.AspNetCore.Authorization; // <--- Add this
[Route("api/[controller]")]
[ApiController]
public class AuthController(ApplicationDbContext context, IAuthService authService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Change _context to context here:
        if (await context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("User already exists.");

        // And here:
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == request.Role.ToLower());

        if (role == null) return BadRequest("Invalid Role.");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            RoleId = role.Id
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok("User Registered!");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // And here:
        var user = await context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = authService.CreateToken(user.Id.ToString(), user.Email, user.Role.Name);

        return Ok(new { Token = token });
    }
}