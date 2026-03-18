namespace SecureAuthApi.Src.Models.DTOs;

public record RegisterRequest(string Email, string Password, string Role);
public record LoginRequest(string Email, string Password);