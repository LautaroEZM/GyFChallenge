namespace GyFChallenge.Models
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public DateOnly DateAdded { get; set; }
        public string? Category { get; set; }
    }

    public class User
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Mail { get; set; }
    }
}
