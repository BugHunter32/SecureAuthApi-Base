using Microsoft.EntityFrameworkCore;
using SecureAuthApi.Src.Models.Entities;

namespace SecureAuthApi.Src.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // 1. Seed Roles
    modelBuilder.Entity<Role>().HasData(
        new Role { Id = 1, Name = "Admin" },
        new Role { Id = 2, Name = "User" }
    );

    // 2. Seed First Admin User
    // Use a known GUID so migrations stay consistent
    var adminId = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d");
    modelBuilder.Entity<User>().HasData(new User
    {
        Id = adminId,
        Email = "admin@company.com",
        // Password is 'Admin123!' - hashed using BCrypt
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"), 
        RoleId = 1 // Admin Role
    });
}
}