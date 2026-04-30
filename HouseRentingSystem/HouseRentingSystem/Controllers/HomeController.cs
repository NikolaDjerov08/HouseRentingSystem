using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Models;
using HouseRentingSystem.Models.Home;
using HouseRentingSystem.Models.House;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View("Error500");
        }

        public IActionResult StatusCodeHandler(int statusCode)
        {
            ViewBag.StatusCode = statusCode;

            if (statusCode == 401 || statusCode == 404)
            {
                return View("ErrorStatus");
            }
            return View("ErrorStatus");
        }
    }
}
