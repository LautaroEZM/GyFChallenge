using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
