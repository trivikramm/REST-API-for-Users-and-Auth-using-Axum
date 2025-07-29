using LoginDashboardAPI.Services;
using LoginDashboardAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LoginDashboardAPI.Middleware;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure JWT authentication
var secretKey = builder.Configuration["JwtSettings:SecretKey"] ?? "your-default-secret-key-for-development-only";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add memory cache for rate limiting
builder.Services.AddMemoryCache();

// Register services
builder.Services.AddScoped<IAuthService>(provider => new AuthService(secretKey));
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // This will allow requests from any origin
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// Use CORS policy
app.UseCors("AllowAll");

// Add rate limiting middleware
app.UseMiddleware<RateLimitMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add a welcome page at the root URL
app.MapGet("/", () => "Welcome to the Login Dashboard API! The API is up and running. Try accessing /swagger to see the API documentation.");

app.Run();
