using System;
using System.ComponentModel.DataAnnotations;

namespace Notifications_System.Models.AskModels
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Question { get; set; }

        public string Answer { get; set; }

        public ApplicationUser Sender { get; set; }

        public string SenderId { get; set; }

        public ApplicationUser Reciever { get; set; }

        public string RecieverId { get; set; }

        public DateTime DateAsked { get; set; }

        public DateTime? DateAnswerd { get; set; }

        public bool IsAnonymously { get; set; }
    }
}