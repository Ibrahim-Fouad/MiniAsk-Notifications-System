using Microsoft.AspNet.Identity.EntityFramework;
using Notifications_System.Models.AskModels;
using Notifications_System.Models.NotificationModels;
using System.Data.Entity;

namespace Notifications_System.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}