using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Models;
using HouseRentingSystem.Models.House;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly HouseRentingDbContext context;

        public HomeController(HouseRentingDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new HomeViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };

            if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(userId))
            {
                model.UserHousesCount = await context.Houses
                    .CountAsync(h => h.AgentId == userId && h.IsDeleted == false);
            }

            return View(model);
        }

        [Route("Home/Error")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode.HasValue)
            {
                switch (statusCode.Value)
                {
                    case 401:
                        return View("Error401");
                    case 404:
                        return View("Error404");
                }
            }
            return View("Error404");
        }

        public IActionResult ServerError()
        {
            return View("Error500");
        }
    }
}
