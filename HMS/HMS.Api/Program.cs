using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HMS.Domain.Entities;
using HMS.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// AutoMapper-ის რეგისტრაცია
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<HMS.Application.Mappings.MappingProfile>();
});
// Repositories-ის რეგისტრაცია
builder.Services.AddScoped(typeof(HMS.Application.Interfaces.IGenericRepository<>), typeof(HMS.Infrastructure.Repositories.GenericRepository<>));
builder.Services.AddScoped<HMS.Application.Interfaces.IHotelRepository, HMS.Infrastructure.Repositories.HotelRepository>();
builder.Services.AddScoped<HMS.Application.Interfaces.IRoomRepository, HMS.Infrastructure.Repositories.RoomRepository>();
builder.Services.AddScoped<HMS.Application.Interfaces.IGuestRepository, HMS.Infrastructure.Repositories.GuestRepository>();
builder.Services.AddScoped<HMS.Application.Interfaces.IReservationRepository, HMS.Infrastructure.Repositories.ReservationRepository>();
builder.Services.AddScoped<HMS.Application.Interfaces.IManagerRepository, HMS.Infrastructure.Repositories.ManagerRepository>();

// Services-ის რეგისტრაცია
builder.Services.AddScoped<HMS.Application.Interfaces.IHotelService, HMS.Application.Services.HotelService>();
builder.Services.AddScoped<HMS.Application.Interfaces.IRoomService, HMS.Application.Services.RoomService>();
builder.Services.AddScoped<HMS.Application.Interfaces.IGuestService, HMS.Application.Services.GuestService>();
builder.Services.AddScoped<HMS.Application.Interfaces.IReservationService, HMS.Application.Services.ReservationService>();
builder.Services.AddScoped<HMS.Application.Interfaces.IManagerService, HMS.Application.Services.ManagerService>();
builder.Services.AddScoped<HMS.Application.Interfaces.IAuthService, HMS.Application.Services.AuthService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Swagger-ს ვეუბნებით, რომ ვიყენებთ Bearer ტოკენს
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "აქ ჩაწერეთ: Bearer {თქვენი_ტოკენი}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// DbContext-ის რეგისტრაცია SQL Server-ით
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Identity-ს რეგისტრაცია
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// ერორების დამჭერი
app.UseMiddleware<HMS.Api.Middlewares.ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
