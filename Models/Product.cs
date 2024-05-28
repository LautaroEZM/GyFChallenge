public enum CategoryEnum
{
    CategoryA,
    CategoryB,
}

namespace GyFChallenge.Models
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateOnly CreatedAt { get; set; }
        public CategoryEnum Category { get; set; }
    }
}
