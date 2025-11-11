using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EcommerceWarehouse.Server.Entities;
using EcommerceWarehouse.Server.Entities.Interfaces;

namespace EcommerceWarehouse.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany()
                .HasForeignKey(rp => rp.RoleId);
            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany()
                .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<Product>().HasIndex(p => p.Sku).IsUnique();
            builder.Entity<Order>().HasIndex(o => o.OrderNumber).IsUnique();

            builder.Entity<OrderDetail>()
                .HasOne(d => d.Order)
                .WithMany(o => o.Details)
                .HasForeignKey(d => d.OrderId);
            builder.Entity<OrderDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId);

            builder.Entity<StockTransaction>()
                .HasOne(t => t.Product)
                .WithMany(p => p.StockTransactions)
                .HasForeignKey(t => t.ProductId);

            // Global query filter for soft delete
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ApplicationDbContext).GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(null, new object[] { builder });
                }
            }

            // Seed via migrations: roles, permissions, role-permissions
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userRoleId  = Guid.Parse("22222222-2222-2222-2222-222222222222");

            builder.Entity<IdentityRole<Guid>>().HasData(
                new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "1" },
                new IdentityRole<Guid> { Id = userRoleId, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "2" }
            );

            var permissions = new List<Permission>
            {
                new() { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"), Module = "User", Action = "View", Description = "View User" },
                new() { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"), Module = "User", Action = "Create", Description = "Create User" },
                new() { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000003"), Module = "User", Action = "Update", Description = "Update User" },
                new() { Id = Guid.Parse("aaaaaaaa-0000-0000-0000-000000000004"), Module = "User", Action = "Delete", Description = "Delete User" },
                new() { Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"), Module = "Role", Action = "View", Description = "View Role" },
                new() { Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"), Module = "Role", Action = "Create", Description = "Create Role" },
                new() { Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000003"), Module = "Role", Action = "Update", Description = "Update Role" },
                new() { Id = Guid.Parse("bbbbbbbb-0000-0000-0000-000000000004"), Module = "Role", Action = "Delete", Description = "Delete Role" }
            };
            builder.Entity<Permission>().HasData(permissions);

            var rolePerms = new List<RolePermission>();
            foreach (var perm in permissions)
            {
                rolePerms.Add(new RolePermission { RoleId = adminRoleId, PermissionId = perm.Id });
            }
            foreach (var perm in permissions.Where(p => p.Action == "View"))
            {
                rolePerms.Add(new RolePermission { RoleId = userRoleId, PermissionId = perm.Id });
            }
            builder.Entity<RolePermission>().HasData(rolePerms);
        }

        private static void SetSoftDeleteFilter<T>(ModelBuilder builder) where T : class, ISoftDelete
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditableEntity auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = utcNow;
                    }
                    if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedAt = utcNow;
                    }
                }
                if (entry.Entity is ISoftDelete soft && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    soft.IsDeleted = true;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
