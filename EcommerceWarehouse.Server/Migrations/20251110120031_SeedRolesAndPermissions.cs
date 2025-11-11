using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceWarehouse.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "1", "Admin", "ADMIN" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "2", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Action", "Description", "Module" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), "View", "View User", "User" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000002"), "Create", "Create User", "User" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000003"), "Update", "Update User", "User" },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000004"), "Delete", "Delete User", "User" },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), "View", "View Role", "Role" },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000002"), "Create", "Create Role", "Role" },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000003"), "Update", "Update Role", "Role" },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000004"), "Delete", "Delete Role", "Role" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000004"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("bbbbbbbb-0000-0000-0000-000000000001"), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-0000-0000-0000-000000000004"));
        }
    }
}
