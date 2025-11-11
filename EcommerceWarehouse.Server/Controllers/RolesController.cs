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
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RolesController(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_roleManager.Roles.Select(r => new { r.Id, r.Name }));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(dto.Name));
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            role.Name = dto.Name;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);
            return NoContent();
        }

        public record RoleDto(string Name);
    }
}
