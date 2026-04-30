using House_renting_system_service.Contracts;
using HouseRentingSystem.Data.Data;
using HouseRentingSystem.Models.House;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House_renting_system_service.Implemetations
{
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext context;
        public HouseService()
        {
            context = new HouseRentingDbContext();
        }
        public async Task<IEnumerable<HouseViewModel>> GetHousesByUserId(string userId)
        {
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
            return houses;
        }
    }
}
