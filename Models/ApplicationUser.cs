using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ApiUser.Models
{
    public class ApplicationUser : IdentityUser
    {
        private const string V = "nvarchar(150)";

        [Column(TypeName = V)]
        public string FullName { get; set;}
    }
}