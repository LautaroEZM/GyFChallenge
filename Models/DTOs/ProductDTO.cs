using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models.DTOs
{
    public class ProductDTO
    {
        [Length(5, 100)]
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(2, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }

        [Required]
        [AllowedValues("Category1", "Category2")]
        public string Category { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Required]
        public int Stock { get; set; }
    }


}
