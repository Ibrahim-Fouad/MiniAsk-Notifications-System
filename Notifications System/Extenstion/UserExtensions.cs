using Microsoft.AspNet.Identity;
using Notifications_System.Models;
using System;
using System.Linq;
using System.Security.Principal;

namespace Notifications_System.Extenstion
{
    public static class UserExtensions
    {
        public static string GetUserFullname(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var userId = identity.GetUserId();
            var db = new ApplicationDbContext();
            var user = db.Users.Single(u => u.Id == userId);

            return user.FullName;
        }

        public static string GetUsername(this IIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException(nameof(identity));

            var userId = identity.GetUserId();
            var db = new ApplicationDbContext();
            var user = db.Users.Single(u => u.Id == userId);

            return user.UniqueUsername;
        }
    }
}