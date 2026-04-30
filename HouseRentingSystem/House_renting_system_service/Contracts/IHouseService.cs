using HouseRentingSystem.Models.House;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace House_renting_system_service.Contracts
{
    public interface IHouseService
    {
        async Task<IEnumerable<HouseViewModel>> GetHousesByUserId(string userId);
    }
}
