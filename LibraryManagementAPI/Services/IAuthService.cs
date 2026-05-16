using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services
{
    public interface IAuthService
    {
        string Register(RegisterDto registerDto);
        string Login(LoginDto loginDto);
    }
}