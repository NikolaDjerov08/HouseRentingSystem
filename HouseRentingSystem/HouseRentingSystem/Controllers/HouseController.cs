using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {
        private List<HouseViewModel> houses = new List<HouseViewModel>()
        {
            new HouseViewModel()
            {
                Name = "Beach house",
                Address = "Miami, Florida",
                ImageUrl = "@https://sdchouseplans.com/wp-content/uploads/2025/04/image5.jpg"
            },
            new HouseViewModel()
            {
                Name = "Mountain house",
                Address = "Rila Mountain, Bulgaria",
                ImageUrl = "@https://bghike.com/en/images/huts_pic/rila_lakes_main.jpg"
            }
        };
        public IActionResult AllHouses()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            return View(houses.FirstOrDefault(h=>h.Id == id);
        }
    }
}
