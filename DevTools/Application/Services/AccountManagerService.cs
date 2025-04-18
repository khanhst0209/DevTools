using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.RegisterException;
using Microsoft.AspNetCore.Identity;
using MyWebAPI.data;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using DevTools.Exceptions.AccountManager.LoginException;
using AutoMapper;
using DevTools.Exceptions.AccountManager.UserException;
using DevTools.Application.Exceptions.AccountManager.ChangePassword;
using DevTools.Application.Dto.user;
using DevTools.Infrastructure.Repositories.Interfaces;


namespace DevTools.Services
{
    public class AccountManagerService : IAccountManagerService
    {

        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signinManager;
        private readonly IMapper _mapper;
        private readonly IPremiumUpgradeRequestRepository _premiumUpgradeRequestRepository;

        public AccountManagerService(UserManager<User> userManager,
         ITokenService tokenService,
         SignInManager<User> _signinManager,
         IMapper _mapper,
         IPremiumUpgradeRequestRepository premiumUpgradeRequestRepository)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
            this._signinManager = _signinManager;
            this._mapper = _mapper;
            _premiumUpgradeRequestRepository = premiumUpgradeRequestRepository;
        }

        public async Task DeleteUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFound(userId);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            var request = await _premiumUpgradeRequestRepository.GetByIdAsync(userId);

            if(request != null)
            {
                await _premiumUpgradeRequestRepository.RemoveAsync(userId);
            }

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

        public async Task<UserDTO> GetUserById(string Id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == Id);

            if (user == null)
                throw new UserNotFound(Id);

            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Role = (await _userManager.GetRolesAsync(user))[0];

            return userDTO;
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

        public async Task PasswordChange(string userId, PasswordChangeDTO passwordChange)
        {
            if (passwordChange.NewPassword != passwordChange.ConfirmNewPassword)
                throw new UnvalidConfirmPasswordException(passwordChange.NewPassword, passwordChange.ConfirmNewPassword);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new UserNotFound(userId);

            var result = await _userManager.ChangePasswordAsync(user, passwordChange.OldPassword, passwordChange.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new PasswordChangeFailedException(errors);
            }

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

        public async Task RoleChange(ChangeRoleDTO changeRole)
        {
            var user = await _userManager.FindByIdAsync(changeRole.UserId);
            if (user == null)
                throw new UserNotFound(changeRole.UserId);

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles[0] == changeRole.Role)
                throw new Exception("New Role is Current Role");


            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                throw new Exception(removeResult.Errors.ToString());

            var addResult = await _userManager.AddToRoleAsync(user, changeRole.Role);
            if (!addResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, currentRoles[0]);
                throw new Exception(addResult.Errors.ToString());
            }

            if(changeRole.Role == "Premium" || changeRole.Role == "Admin")
            {
                var request = await _premiumUpgradeRequestRepository.GetByIdAsync(changeRole.UserId);

                if(request != null)
                {
                    await _premiumUpgradeRequestRepository.RemoveAsync(request.UserId);
                }
            }
        }
    }
}