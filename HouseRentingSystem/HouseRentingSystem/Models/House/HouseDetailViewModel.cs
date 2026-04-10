namespace HouseRentingSystem.Models.House
{
    public class HouseDetailViewModel : HouseViewModel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
    }
}
