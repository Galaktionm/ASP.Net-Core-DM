using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.DTO;
using UserService.Services;

namespace UserService.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly JWTService service;

        public AccountController(DatabaseContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, JWTService JWTService)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            service = JWTService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new User(request.userName, request.email);
            var result = await userManager.CreateAsync(user, request.password);

            if (result.Succeeded)
            {
                var addRole = await userManager.AddToRoleAsync(user, "User");
                if (addRole.Succeeded == false)
                {
                    return BadRequest(new { message = "There was an error processing your request" });
                }
            }
            else
            {
                return BadRequest(new { message = "There was an error processing your request" });
            }

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null
                || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid Email or Password."
                });
            var secToken = await service.GetTokenAsync(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt
            });
        }
    }
}

