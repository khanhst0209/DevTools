using DevTools.Dto.user;
using MyWebAPI.Dto.user;

namespace MyWebAPI.Services.Interfaces
{
    public interface IAccountManagerService
    {
        Task<NewUserDTO> Login(LoginDTO logindto);
        Task<NewUserDTO> Register(RegisterDTO registerDto);
    }
} 