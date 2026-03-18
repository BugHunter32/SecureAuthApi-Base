using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureAuthApi.Src.Data;
using SecureAuthApi.Src.Middleware;
using SecureAuthApi.Src.Services;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// 1. Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Auth & JWT
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        
        // CRITICAL FIX: Explicitly map the Role and Name claims
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Email
    };
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Standard Swagger for now

var app = builder.Build();

// 3. Middleware Pipeline (Order is everything!)
app.UseMiddleware<ApiKeyMiddleware>(); // First gate: API Key
app.UseRouting();
app.UseAuthentication();               // Second gate: JWT valid?
app.UseAuthorization();                // Third gate: Has correct Role?

app.MapControllers();
app.Run();