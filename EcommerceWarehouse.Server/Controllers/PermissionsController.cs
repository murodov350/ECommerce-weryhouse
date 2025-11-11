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
    public class PermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public PermissionsController(ApplicationDbContext db, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _db = db; _roleManager = roleManager;
        }

        [HttpGet("catalog")]
        public async Task<IActionResult> GetCatalog()
        {
            var list = await _db.Permissions.AsNoTracking().ToListAsync();
            return Ok(list);
        }

        [HttpPost("catalog")]
        public async Task<IActionResult> UpsertPermission([FromBody] Permission permission)
        {
            if (permission.Id == Guid.Empty)
            {
                permission.Id = Guid.NewGuid();
                _db.Permissions.Add(permission);
            }
            else
            {
                _db.Permissions.Update(permission);
            }
            await _db.SaveChangesAsync();
            return Ok(permission);
        }

        [HttpDelete("catalog/{id:guid}")]
        public async Task<IActionResult> DeletePermission(Guid id)
        {
            var entity = await _db.Permissions.FindAsync(id);
            if (entity == null) return NotFound();
            _db.Permissions.Remove(entity);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public record AssignRequest(Guid RoleId, Guid[] PermissionIds);

        [HttpPost("assign")]
        public async Task<IActionResult> AssignToRole([FromBody] AssignRequest req)
        {
            var role = await _roleManager.FindByIdAsync(req.RoleId.ToString());
            if (role == null) return NotFound("Role not found");

            var existing = _db.RolePermissions.Where(x => x.RoleId == req.RoleId);
            _db.RolePermissions.RemoveRange(existing);

            var perms = await _db.Permissions.Where(p => req.PermissionIds.Contains(p.Id)).ToListAsync();
            foreach (var p in perms)
            {
                _db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = p.Id });
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("role/{roleId:guid}")]
        public async Task<IActionResult> GetPermissionsForRole(Guid roleId)
        {
            var ids = await _db.RolePermissions.Where(x => x.RoleId == roleId).Select(x => x.PermissionId).ToListAsync();
            return Ok(ids);
        }
    }
}
