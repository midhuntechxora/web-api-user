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

        [HttpGet]
        [Authorize(Roles="Admin")]
        [Route("forAdmin")]
        //GET api/forAdmin
        public string GetForAdmin() {
            return "Web method for admin";
        }

        [HttpGet]
        [Authorize(Roles="User")]
        [Route("forUser")]
        //GET api/forUser
        public string GetForUser() {
            return "Web method for User";
        }

        [HttpGet]
        [Authorize(Roles="Admin,User")]
        [Route("forAdminOrUser")]
        //GET api/forAdminOrUser
        public string GetAdminOrUser() {
            return "Web method for AdminOrUser";
        }
        
        [HttpPut]
        [Authorize]
        //PUT api/User
        public async Task<Object> UpdateUserProfile(ApplicationUserModel model) {
            string userId = User.Claims.First(c =>c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            user.FullName=model.FullName;
            user.Email=model.Email;
            IdentityResult identityResult = await _userManager.UpdateAsync(user);
            return new  {
                user.FullName,
                user.Email,
                user.UserName
            };
        }

    }
}