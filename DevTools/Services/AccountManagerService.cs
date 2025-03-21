using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.RegisterException;
using Microsoft.AspNetCore.Identity;
using MyWebAPI.data;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DevTools.Services
{
    public class AccountManagerService : IAccountManagerService
    {

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;

        public AccountManagerService(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> _signinManager)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signinManager = _signinManager;
        }
        public async Task<NewUserDTO> Login(LoginDTO logindto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == logindto.username);
            Console.WriteLine("check 1");
            if (user == null)
            {
                return null;
            }
            Console.WriteLine("check 1");

            var result = await _signinManager.CheckPasswordSignInAsync(user, logindto.password, false);

            if (!result.Succeeded)
                return null;
            Console.WriteLine("check 2");
            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine("check 3");
            return new NewUserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user, roles)
            };

        }

        public async Task<NewUserDTO> Register(RegisterDTO registerDto)
        {
            var appuser = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appuser, registerDto.Password);
            if (!createdUser.Succeeded)
            {
                throw new UserCreationFailedException("User creation failed: " +
                    string.Join(", ", createdUser.Errors.Select(e => e.Description)));
            }

            var roleUser = await _userManager.AddToRoleAsync(appuser, "User");
            if (!roleUser.Succeeded)
            {
                await _userManager.DeleteAsync(appuser);
                throw new RoleAssignmentFailedException("Role assignment failed: " +
                    string.Join(", ", roleUser.Errors.Select(e => e.Description)));
            }
            var roles = await _userManager.GetRolesAsync(appuser);
            return new NewUserDTO
            {
                UserName = appuser.UserName,
                Email = appuser.Email,
                Token = _tokenService.CreateToken(appuser, roles)
            };
        }

    }
}