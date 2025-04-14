using DevTools.Application.Dto.user;
using DevTools.Dto.user;
using MyWebAPI.Dto.user;

namespace MyWebAPI.Services.Interfaces
{
    public interface IAccountManagerService
    {
        Task<NewUserDTO> Login(LoginDTO logindto);
        Task<NewUserDTO> Register(RegisterDTO registerDto);
        Task<List<UserDTO>> GetAllUsers();

        Task<UserDTO> GetUserById(string Id);

        Task PasswordChange(string userId, PasswordChangeDTO passwordChange);
        Task RoleChange(ChangeRoleDTO changeRole);

        Task DeleteUserById(string userId);
    }
}