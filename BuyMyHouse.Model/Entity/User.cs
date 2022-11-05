namespace BuyMyHouse.Model.Entity
{
    public class User
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? DeletedAt { get; set; }
        public double? SalaryPerYear { get; set; }
        public double? MortgageOffer { get; set; }


    }
}

