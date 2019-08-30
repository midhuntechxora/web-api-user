using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiUser.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IOptions<ApplicationSettings> appSettings) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }
        [HttpPost]
        [Route("Register")]
        //POST : api/ApplicationUser/Register
        public async Task<Object> CreateUser(ApplicationUserModel model) {
            var applicationUser = new ApplicationUser() {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };
            try
            {
                var result = await _userManager.CreateAsync(applicationUser,model.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        [HttpPost]
        [Route("Login")]
        //POST : api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model){
            var user = await _userManager.FindByNameAsync(model.UserName);
            var key = Encoding.UTF8.GetBytes(_appSettings.JWT_Secret);
            if(user !=  null && await _userManager.CheckPasswordAsync(user,model.Password)) {
                   var tokenDescriptor = new SecurityTokenDescriptor{
                       Subject = new ClaimsIdentity (new Claim[] {
                           new Claim("UserID",user.Id.ToString())
                       }),
                       Expires = DateTime.UtcNow.AddMinutes(5),
                       SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                   }; 
                   var tokenHandler = new JwtSecurityTokenHandler();
                   var securityToken =tokenHandler.CreateToken(tokenDescriptor);
                   var token = tokenHandler.WriteToken(securityToken);
                   return Ok(new {token});
            }  else {
                return BadRequest(new { message = "Username or password is incorrect"});
            }
        }



    }
}