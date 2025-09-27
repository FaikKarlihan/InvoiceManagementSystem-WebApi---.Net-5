using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Entities
{
    [Index(nameof(Token), IsUnique = true)]
    public class RevokedToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = null!;
        
        public DateTime RevokedAt { get; set; } = DateTime.UtcNow;
    }
}