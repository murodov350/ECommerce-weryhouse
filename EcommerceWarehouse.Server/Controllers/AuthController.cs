using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceWarehouse.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtOptions.Value;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { error = "Email and password required" });

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new { error = "User not found" });

            if (!user.IsActive)
                return BadRequest(new { error = "User inactive" });

            var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
                return BadRequest(new { error = "Invalid password" });

            var token = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { token, roles });
        }

        [HttpGet("debug-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> DebugAdmin()
        {
            var admin = await _userManager.FindByEmailAsync("admin@ew.local");
            if (admin == null) return Ok(new { exists = false });
            var roles = await _userManager.GetRolesAsync(admin);
            var passwordOk = await _userManager.CheckPasswordAsync(admin, "123456");
            return Ok(new { exists = true, roles, passwordOk });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginRequest
        {
            public string Email { get; set; } = default!;
            public string Password { get; set; } = default!;
        }
    }

    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 60;
    }
}
