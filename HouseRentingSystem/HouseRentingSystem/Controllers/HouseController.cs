using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Models;
using HouseRentingSystem.Models.House;
using HouseRentingSystem.Models.House.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {
        private readonly HouseRentingDbContext context;

        public HouseController(HouseRentingDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> AllHouses([FromQuery] AllHousesQueryModel query)
        {
            var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (query.CurrentPage < 1)
            {
                query.CurrentPage = 1;
            }

            var housesQuery = context.Houses
                .AsNoTracking()
                .Where(h => h.IsDeleted == false)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string search = query.SearchTerm.ToLower();

                housesQuery = housesQuery.Where(h =>
                    h.Title.ToLower().Contains(search) ||
                    h.Address.ToLower().Contains(search) ||
                    h.Description.ToLower().Contains(search));
            }

            housesQuery = query.Sorting switch
            {
                HouseSorting.Price => housesQuery.OrderBy(h => h.PricePerMonth),

                HouseSorting.NotRentedFirst => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),

                _ => housesQuery.OrderByDescending(h => h.Id)
            };

            query.TotalHousesCount = await housesQuery.CountAsync();

            query.Houses = await housesQuery
                .Skip((query.CurrentPage - 1) * AllHousesQueryModel.HousesPerPage)
                .Take(AllHousesQueryModel.HousesPerPage)
                .Select(h => new HouseViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    CurentUserIsOwner = h.AgentId == currentUsersId
                })
                .ToListAsync();

            query.Categories = await context.Categories
                .AsNoTracking()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();

            ViewBag.Title = "All houses";

            return View(query);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var searched = await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == Id);

            var model = new HouseDetailViewModel()
            {
                Id = searched.Id,
                Address = searched.Address,
                ImageUrl = searched.ImageUrl,
                Description = searched.Description,
                CreatedBy = searched.Agent.UserName,
                Price = searched.PricePerMonth,
                Name = searched.Title
            };

            return View(model);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateHouse()
        {
            List<CategoryViewModel> ListOfCategories = await context.Categories
            .AsNoTracking()
            .Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToListAsync();
            var houseCategories = new HouseFormViewModel()
            {
                Categories = ListOfCategories
            };
            return View(houseCategories);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateHouse(HouseFormViewModel model)
        {

            var houseCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            if (!ModelState.IsValid)
            {

                model.Categories = houseCategories;
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool addressExists = await context.Houses
                .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower());

            if (addressExists)
            {
                model.Categories = houseCategories;
                ModelState.AddModelError("Address", "This address is already registered");
                return View(model);
            }

            var newHouse = new House
            {
                Title = model.Title,
                Address = model.Address,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PricePerMonth = model.PricePerMonth,
                CategoryId = model.SelectedCategoryId,
                AgentId = userId
            };

            context.Houses.Add(newHouse);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyHouses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var houses = await context.Houses
                .AsNoTracking()
                .Where(h => h.AgentId == userId && h.IsDeleted == false)
                .Select(h => new HouseViewModel
                {
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Name = h.Title,
                    Id = h.Id,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    CurentUserIsOwner = true
                })
                .ToListAsync();

            var model = new AllHousesQueryModel
            {
                Houses = houses,
                TotalHousesCount = houses.Count
            };

            ViewBag.Title = "My houses";

            return View(nameof(AllHouses), model);
        }
    }
}
