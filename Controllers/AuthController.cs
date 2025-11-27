using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SchoolAPI.Models;
using SchoolAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly UserService _userService;
		private readonly IConfiguration _configuration;

		public AuthController(UserService userService, IConfiguration configuration)
		{
			_userService = userService;
			_configuration = configuration;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(LoginModel model)
		{
			if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
			{
				return BadRequest("Username and password are required");
			}

			if (await _userService.UserExistsAsync(model.Username))
			{
				return BadRequest("Username already exists");
			}

			var user = new User
			{
				Username = model.Username,
				PasswordHash = PasswordHasher.HashPassword(model.Password),
				Role = "Admin" // You can change this to have different roles
			};

			await _userService.CreateUserAsync(user);

			return Ok(new { message = "User registered successfully" });
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			var user = await _userService.GetUserByUsernameAsync(model.Username);
			if (user == null || !PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
			{
				return Unauthorized("Invalid username or password");
			}

			var token = GenerateJwtToken(user);
			return Ok(new { token = token, username = user.Username, role = user.Role });
		}

		private string GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("Jwt");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role),
				new Claim(ClaimTypes.NameIdentifier, user.Id ?? "")
			};

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}