using Notifications_System.Models;
using Notifications_System.Models.AskModels;
using System.Collections.Generic;

namespace Notifications_System.ViewModels
{
    public class UsernameFormViewModel
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}