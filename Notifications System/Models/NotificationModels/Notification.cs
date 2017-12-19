using Notifications_System.Models.AskModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Notifications_System.Models.NotificationModels
{
    public class Notification
    {
        public int Id { get; set; }


        public DateTime DateCreated { get; set; }

        public ApplicationUser Sender { get; set; }

        public ApplicationUser Reciever { get; set; }



        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        public bool IsRead { get; set; }


        public string SenderId { get; set; }

        [Required]
        public string RecieverId { get; set; }

        public Post Post { get; set; }

        public int PostId { get; set; }
    }
}