using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IMemberRepository memberRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _configuration = configuration;
        }

        public string Register(RegisterDto registerDto)
        {
            if (_memberRepository.EmailExists(registerDto.Email))
                return "Email already exists.";

            var member = new Member
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = "Member",
                CreatedAt = DateTime.UtcNow
            };

            _memberRepository.Add(member);
            return GenerateToken(member);
        }

        public string Login(LoginDto loginDto)
        {
            var member = _memberRepository.GetByEmail(loginDto.Email);
            if (member == null) return "Invalid email or password.";

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, member.PasswordHash))
                return "Invalid email or password.";

            return GenerateToken(member);
        }

        private string GenerateToken(Member member)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
                new Claim(ClaimTypes.Email, member.Email),
                new Claim(ClaimTypes.Name, member.FullName),
                new Claim(ClaimTypes.Role, member.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpiryInMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}