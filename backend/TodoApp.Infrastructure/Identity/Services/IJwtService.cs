using TodoApp.Domain.Identity.Entities;

namespace TodoApp.Infrastructure.Identity.Services;

// Interface cho JWT Token Service
public interface IJwtService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? ValidateAccessToken(string token);
    JwtSettings Settings { get; }
}

// JWT Configuration Settings
public class JwtSettings
{
    public string SecretKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpireMinutes { get; set; } = 60;
    public int RefreshTokenExpireDays { get; set; } = 7;
}
