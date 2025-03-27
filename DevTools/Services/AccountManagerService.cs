using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.RegisterException;
using Microsoft.AspNetCore.Identity;
using MyWebAPI.data;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using DevTools.Exceptions.AccountManager.LoginException;
using AutoMapper;


namespace DevTools.Services
{
    public class AccountManagerService : IAccountManagerService
    {

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly IMapper _mapper;

        public AccountManagerService(UserManager<User> userManager,
         ITokenService tokenService,
         SignInManager<User> _signinManager,
         IMapper _mapper)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signinManager = _signinManager;
            this._mapper = _mapper;
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = _mapper.Map<List<UserDTO>>(users);

            foreach (var userDto in userDtos)
            {
                var user = users.FirstOrDefault(u => u.Id == userDto.Id);
                if (user != null)
                {
                    userDto.Role = (await _userManager.GetRolesAsync(user))[0];
                }
            }

            return userDtos;
        }


        public async Task<NewUserDTO> Login(LoginDTO logindto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == logindto.UserName);
            if (user == null)
            {
                throw new InvalidUsernameOrPassword(logindto.UserName, logindto.password);
            }

            var result = await _signinManager.CheckPasswordSignInAsync(user, logindto.password, false);

            if (!result.Succeeded)
                throw new InvalidUsernameOrPassword(logindto.UserName, logindto.password);
            var roles = await _userManager.GetRolesAsync(user);

            return new NewUserDTO
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user, roles)
            };

        }

        public async Task<NewUserDTO> Register(RegisterDTO registerDto)
        {
            var appuser = new User
            {
                FullName = registerDto.FullName,
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                IsPremium = false
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
                FullName = appuser.FullName,
                UserName = appuser.UserName,
                Email = appuser.Email,
                Token = _tokenService.CreateToken(appuser, roles)
            };
        }

    }
}