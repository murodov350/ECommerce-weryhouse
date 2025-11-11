using EcommerceWarehouse.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userManager.Users.Select(u => new { u.Id, u.Email, u.FirstName, u.LastName, u.IsActive }).ToListAsync();
            return Ok(users);
        }

        public record UpsertUser(Guid? Id, string Email, string? FirstName, string? LastName, bool IsActive, string? Password, Guid[] RoleIds);

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UpsertUser req)
        {
            ApplicationUser? user = null;
            if (req.Id.HasValue)
            {
                user = await _userManager.FindByIdAsync(req.Id.ToString()!);
                if (user == null) return NotFound();
                user.Email = req.Email;
                user.UserName = req.Email;
                user.FirstName = req.FirstName;
                user.LastName = req.LastName;
                user.IsActive = req.IsActive;
                var updateRes = await _userManager.UpdateAsync(user);
                if (!updateRes.Succeeded) return BadRequest(updateRes.Errors);
                if (!string.IsNullOrWhiteSpace(req.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passRes = await _userManager.ResetPasswordAsync(user, token, req.Password!);
                    if (!passRes.Succeeded) return BadRequest(passRes.Errors);
                }
            }
            else
            {
                user = new ApplicationUser
                {
                    Email = req.Email,
                    UserName = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    IsActive = req.IsActive,
                    EmailConfirmed = true
                };
                var createRes = await _userManager.CreateAsync(user, req.Password ?? "123456");
                if (!createRes.Succeeded) return BadRequest(createRes.Errors);
            }

            // roles
            var allRoles = _roleManager.Roles.ToList();
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any()) await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var toAdd = allRoles.Where(r => req.RoleIds.Contains(r.Id)).Select(r => r.Name!).ToArray();
            if (toAdd.Any()) await _userManager.AddToRolesAsync(user, toAdd);

            return Ok(new { user.Id });
        }
    }
}
