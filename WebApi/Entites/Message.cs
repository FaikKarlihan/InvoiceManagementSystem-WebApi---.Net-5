using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Message
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }
    }
}