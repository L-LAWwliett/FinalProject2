using HMS.Application.DTOs.Auth;
using HMS.Application.Interfaces;
using HMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1. ვამოწმებთ, ხომ არ არსებობს უკვე იუზერი ამ მეილით
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return new AuthResponseDto { IsSuccess = false, Message = "მომხმარებელი ამ ელფოსტით უკვე არსებობს." };

            // 2. ვქმნით ახალ იუზერს
            var user = new ApplicationUser
            {
                UserName = registerDto.Email, // ლოგინისთვის მეილს ვიყენებთ
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return new AuthResponseDto { IsSuccess = false, Message = "რეგისტრაცია ვერ მოხერხდა. შეამოწმეთ პაროლის სირთულე." };

            // 3. როლის შემოწმება და მინიჭება (თუ არ არსებობს როლი ბაზაში, ჯერ შექმნის)
            if (!await _roleManager.RoleExistsAsync(registerDto.Role))
                await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));

            await _userManager.AddToRoleAsync(user, registerDto.Role);

            return new AuthResponseDto { IsSuccess = true, Message = "მომხმარებელი წარმატებით დარეგისტრირდა!" };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. ვეძებთ იუზერს მეილით
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return new AuthResponseDto { IsSuccess = false, Message = "არასწორი ელფოსტა ან პაროლი." };

            // 2. ვამოწმებთ პაროლს
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
                return new AuthResponseDto { IsSuccess = false, Message = "არასწორი ელფოსტა ან პაროლი." };

            // 3. თუ ყველაფერი რიგზეა, ვაგენერირებთ ტოკენს
            var token = await GenerateJwtTokenAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "წარმატებული ავტორიზაცია",
                Token = token
            };
        }

        // --- შიდა მეთოდი ტოკენის შესაქმნელად ---
        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Key"];

            // ვამზადებთ ინფორმაციას (Claims), რაც ტოკენში უნდა ჩაიდოს
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // უნიკალური ID ტოკენისთვის
                new Claim("FirstName", user.FirstName)
            };

            // ვამატებთ იუზერის როლებს ტოკენში
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // ვშიფრავთ ჩვენი საიდუმლო გასაღებით
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}