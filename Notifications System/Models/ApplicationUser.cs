using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Notifications_System.Models.AskModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notifications_System.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string UniqueUsername { get; set; }

        [Required]
        public string FullName { get; set; }


        //public ICollection<Post> Posts { get; private set; }

        public ApplicationUser()
        {
           // Posts = new Collection<Post>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}