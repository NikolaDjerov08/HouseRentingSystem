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
        public async Task<IActionResult> AllHouses([FromQuery] QueryViewModel model)
        {
            var currentUsersId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var query = context.Houses
                .AsNoTracking()
                .Where(h => h.IsDeleted == false);

            if (model.CurentCategory != null && model.CurentCategory.Id > 0)
            {
                query = query.Where(h => h.CategoryId == model.CurentCategory.Id);
            }

            if (!string.IsNullOrWhiteSpace(model.KeyWordText))
            {
                query = query.Where(h => h.Title.Contains(model.KeyWordText) ||
                                         h.Address.Contains(model.KeyWordText));
            }

            if (model.IsDecending)
            {
                query = query.OrderByDescending(h => h.Title);
            }
            else
            {
                query = query.OrderBy(h => h.Title);
            }

            var housesViewModel = await query
                .Select(h => new HousesViewModel
                {
                    Id = h.Id,
                    Name = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    CurentUserIsOwner = h.AgentId == currentUsersId
                })
                .ToListAsync();

            var allHouseViewModel = new AllHouseViewModel
            {
                Houses = housesViewModel,
                Query = new QueryViewModel
                {
                    AllCategory = allCategories,
                    CurentCategory = model.CurentCategory,
                    KeyWordText = model.KeyWordText,
                    IsDecending = model.IsDecending
                }
            };

            ViewBag.Title = "All houses";
            return View(allHouseViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var searched = await context.Houses
                .Include(h => h.Agent)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == Id && h.IsDeleted == false);

            if (searched == null)
            {
                return RedirectToAction("Error404", "Home");
            }

            var model = new HouseDitailViewModel()
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
            ModelState.Remove("Categories");

            if (!ModelState.IsValid)
            {
                var houseCategories = await GetCategoriesAsync();
                model.Categories = houseCategories;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }

                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            bool addressExists = await context.Houses
                .AnyAsync(h => h.Address.ToLower() == model.Address.ToLower() && h.IsDeleted == false);

            if (addressExists)
            {
                var houseCategories = await GetCategoriesAsync();
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
                AgentId = userId,
                IsDeleted = false
            };

            await context.Houses.AddAsync(newHouse);
            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "House created successfully!";
            return RedirectToAction(nameof(AllHouses));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyHouses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var houses = await context.Houses
                .Where(h => h.AgentId == userId && h.IsDeleted == false)
                .Select(h => new HouseViewModel
                {
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    Name = h.Title,
                    Id = h.Id,
                    CurentUserIsOwner = true
                })
                .ToListAsync();

            var allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            var allHouseViewModel = new AllHouseViewModel
            {
                Houses = houses,
                Query = new QueryViewModel
                {
                    AllCategory = allCategories,
                    IsDecending = false
                }
            };

            ViewBag.Title = "My houses";
            return View("AllHouses", allHouseViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var house = await context.Houses
                .FirstOrDefaultAsync(h => h.Id == id && h.IsDeleted == false);

            if (house == null)
            {
                return RedirectToAction("Error404", "Home");
            }

            var houseCategories = await GetCategoriesAsync();

            var model = new HouseFormViewModel()
            {
                Id = house.Id,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                Description = house.Description,
                Title = house.Title,
                PricePerMonth = house.PricePerMonth,
                Categories = houseCategories,
                SelectedCategoryId = house.CategoryId
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HouseFormViewModel model)
        {
            ModelState.Remove("Categories");

            if (!ModelState.IsValid)
            {
                var houseCategories = await GetCategoriesAsync();
                model.Categories = houseCategories;

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }

                return View(model);
            }

            var house = await context.Houses.FindAsync(model.Id);
            if (house == null || house.IsDeleted == true)
            {
                return RedirectToAction("Error404", "Home");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (house.AgentId != userId)
            {
                return RedirectToAction("Error401", "Home");
            }

            house.PricePerMonth = model.PricePerMonth;
            house.Address = model.Address;
            house.ImageUrl = model.ImageUrl;
            house.Description = model.Description;
            house.Title = model.Title;
            house.CategoryId = model.SelectedCategoryId;

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = "House updated successfully!";
            return RedirectToAction(nameof(MyHouses));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var house = await context.Houses.FindAsync(id);
            if (house != null && !house.IsDeleted)
            {
                house.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyHouses));
        }

        private async Task<List<CategoryViewModel>> GetCategoriesAsync()
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }
    }
}
