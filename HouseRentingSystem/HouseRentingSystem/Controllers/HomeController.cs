using HouseRentingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 401 || statusCode == 404)
            {
                ViewBag.StatusCode = statusCode;
                return View("ErrorStatus");
            }

            // fallback (still needed for stability)
            return View("ErrorStatus");
        }
    }
}
