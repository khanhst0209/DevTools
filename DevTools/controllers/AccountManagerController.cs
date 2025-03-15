using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.data;
using MyWebAPI.Dto;
using MyWebAPI.Dto.user;
using MyWebAPI.Services.Interfaces;

namespace MyWebAPI.controllers
{
    [Route("Account")]
    [ApiController]
    public class AccountManagerController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public AccountManagerController(UserManager<User> userManager, ITokenService tokenService)
        {
            this._userManager = userManager;
            this._tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try{
                if(!ModelState.IsValid)
                {
                    return BadRequest(registerDto);
                }

                var appuser = new User{
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                var createdUser = await _userManager.CreateAsync(appuser, registerDto.Password);
                if(createdUser.Succeeded)
                {
                    var roleUser = await _userManager.AddToRoleAsync(appuser, "User");
                    if(roleUser.Succeeded)
                    {
                        return Ok(
                            new NewUserDTO
                            {
                                UserName = appuser.UserName,
                                Email = appuser.Email,
                                Token = _tokenService.CreateToken(appuser)
                            }
                        );
                    }
                    else
                    {
                        return StatusCode(500, roleUser.Errors);
                    }
                }
                else
                    return StatusCode(500, createdUser.Errors);
            }
            catch(Exception e){
                return StatusCode(500, e.Message);
            }

        }
    }
}