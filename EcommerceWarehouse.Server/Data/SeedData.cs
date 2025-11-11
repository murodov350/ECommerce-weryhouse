using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider rootServices)
        {
            using var scope = rootServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            // Apply migrations (creates DB if not exists)
            await context.Database.MigrateAsync();

            // Roles
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            // Admin user
            const string adminEmail = "admin@ew.local";
            const string adminPassword = "123456";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "System",
                    LastName = "Admin",
                    IsActive = true
                };
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            else
            {
                // Ensure admin password matches dev default
                if (!await userManager.CheckPasswordAsync(admin, adminPassword))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                    await userManager.ResetPasswordAsync(admin, token, adminPassword);
                }
                // Ensure role
                var rolesAdmin = await userManager.GetRolesAsync(admin);
                if (!rolesAdmin.Contains("Admin"))
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Regular user
            const string userEmail = "user@ew.local";
            const string userPassword = "123456";
            var regularUser = await userManager.FindByEmailAsync(userEmail);
            if (regularUser is null)
            {
                regularUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    FirstName = "Demo",
                    LastName = "User",
                    IsActive = true
                };
                var createUser = await userManager.CreateAsync(regularUser, userPassword);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }
            else
            {
                if (!await userManager.CheckPasswordAsync(regularUser, userPassword))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(regularUser);
                    await userManager.ResetPasswordAsync(regularUser, token, userPassword);
                }
                var rolesUser = await userManager.GetRolesAsync(regularUser);
                if (!rolesUser.Contains("User"))
                {
                    await userManager.AddToRoleAsync(regularUser, "User");
                }
            }

            // Seed base permissions (CRUD for User & Role)
            var modules = new[] { "User", "Role" };
            var actions = new[] { "View", "Create", "Update", "Delete" };
            var existingPermissions = await context.Permissions.ToListAsync();
            var newPermissions = new List<Permission>();
            foreach (var m in modules)
            {
                foreach (var a in actions)
                {
                    if (!existingPermissions.Any(p => p.Module == m && p.Action == a))
                    {
                        newPermissions.Add(new Permission
                        {
                            Id = Guid.NewGuid(),
                            Module = m,
                            Action = a,
                            Description = $"{a} {m}"
                        });
                    }
                }
            }
            if (newPermissions.Any())
            {
                context.Permissions.AddRange(newPermissions);
                await context.SaveChangesAsync();
            }

            // Assign permissions to roles if not already assigned
            var allPermissions = await context.Permissions.ToListAsync();
            var adminRole = await roleManager.Roles.FirstAsync(r => r.Name == "Admin");
            var userRole = await roleManager.Roles.FirstAsync(r => r.Name == "User");

            var existingRolePerms = await context.RolePermissions.ToListAsync();

            // Admin gets all permissions
            foreach (var perm in allPermissions)
            {
                if (!existingRolePerms.Any(rp => rp.RoleId == adminRole.Id && rp.PermissionId == perm.Id))
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = adminRole.Id, PermissionId = perm.Id });
                }
            }

            // User gets only View permissions for now
            foreach (var perm in allPermissions.Where(p => p.Action == "View"))
            {
                if (!existingRolePerms.Any(rp => rp.RoleId == userRole.Id && rp.PermissionId == perm.Id))
                {
                    context.RolePermissions.Add(new RolePermission { RoleId = userRole.Id, PermissionId = perm.Id });
                }
            }

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
