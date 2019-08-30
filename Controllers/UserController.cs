using System;
using System.Linq;
using System.Threading.Tasks;
using ApiUser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        //GET api/User
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c =>c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new  {
                user.FullName,
                user.Email,
                user.UserName
            };
        }

    }
}