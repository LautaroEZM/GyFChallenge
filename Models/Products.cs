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

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
    }
}
