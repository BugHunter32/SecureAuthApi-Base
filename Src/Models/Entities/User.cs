using System.ComponentModel.DataAnnotations;

namespace SecureAuthApi.Src.Models.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
}