using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email {  get; set; }

        [Required]
        public string Password { get; set; }
    }


}
