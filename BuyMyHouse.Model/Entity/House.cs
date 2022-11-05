namespace BuyMyHouse.Model.Entity
{
    public class House
    {
        public Guid HouseId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? SoldAt { get; set; }

    }
}
