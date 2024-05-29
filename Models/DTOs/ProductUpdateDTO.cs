using System.ComponentModel.DataAnnotations;

namespace GyFChallenge.Models.DTOs
{
    public class ProductUpdateDTO
    {
        [Length(5, 100)]
        public string Name { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Price { get; set; }

        [AllowedValues("Category1", "Category2")]
        public string Category { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Stock { get; set; }
    }


}
