using HouseRentingSystem.Models.House.Helpers;

namespace HouseRentingSystem.Models.Query
{
    public class QueryViewModel
    {
        public List<CategoryViewModel>? AllCategory { get; set; }
        public CategoryViewModel? CurentCategory { get; set; }
        public string? KeyWordText { get; set; }
        public bool IsDecending { get; set; }
    }
}
