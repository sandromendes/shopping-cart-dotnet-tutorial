using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InsertAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminId = Guid.NewGuid();
            var username = "admin";
            var role = "Admin";
            var password = "AdminPassword123";

            // Criar uma instância do PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            // Criar um objeto User para calcular o hash da senha
            var user = new User
            {
                Id = adminId,
                Username = username,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            // Hash da senha
            var passwordHash = passwordHasher.HashPassword(user, password);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Username", "PasswordHash", "Role", "CreatedAt" },
                values: new object[] { adminId, username, passwordHash, role, DateTime.UtcNow }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin"
            );
        }
    }
}
