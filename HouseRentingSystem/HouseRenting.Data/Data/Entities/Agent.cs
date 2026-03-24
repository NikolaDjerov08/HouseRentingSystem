using HouseRentingSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentingSystem.Data.Data.DataConstants.Agent;

namespace HouseRentingSystem.Data.Data.Entities
{
    public class Agent
    {
        public const int PhoneNumberMaxLength = 15;

        public Guid Id { get; init; } = Guid.NewGuid();

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        public IdentityUser User { get; init; } = null!;

        public IEnumerable<House> ManagedHouses { get; set; } = new List<House>();
    }
}
