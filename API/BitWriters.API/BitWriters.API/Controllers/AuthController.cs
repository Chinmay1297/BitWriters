using BitWriters.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BitWriters.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        //POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            //check valid email
            var identityUser = await userManager.FindByEmailAsync(request.Email);
            if(identityUser is not null)
            {
                //check password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    //Create a Token and Response

                    var response = new loginResponseDto(){
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = "TOKEN"
                    };

                    return Ok();
                }
            }

            ModelState.AddModelError("", "Email or Password is incorrect");

            return ValidationProblem(ModelState);
        }

        //POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto requestDto)
        {
            //Create IdentityUser object

            var user = new IdentityUser
            {
                UserName = requestDto.Email?.Trim(),
                Email = requestDto.Email?.Trim()
            };


            var identityResult = await userManager.CreateAsync(user, requestDto.Password);

            if(identityResult.Succeeded)        
            {
                //if user creation succeedes then assign (Reader) role to user
                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if(identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if(identityResult.Errors.Any())
                {
                    foreach(var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
