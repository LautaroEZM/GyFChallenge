using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        
        public int Id { get; set; }

        [Length(5, 50)]
        [Required]
        
        public string Username { get; set; }

        [Length(5, 50)]
        [Required]
        
        public string Email { get; set; }

        [Length(5, 50)]
        [Required]
        public string Password { get; set; }
    }
}
