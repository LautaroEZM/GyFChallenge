using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Product
    {
        public int Id { get; set; }

        [Length(5, 100)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }

        public DateOnly CreatedAt { get; set; } = new DateOnly();

        [Required]
        [AllowedValues("Category1", "Category2")]
        public string Category { get; set; }

        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Stock { get; set; }
    }
}
