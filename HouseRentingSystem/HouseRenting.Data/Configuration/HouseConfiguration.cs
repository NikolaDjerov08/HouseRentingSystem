using HouseRentingSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Data.Configuration
{
    public class HouseConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.HasOne(h => h.Category).WithMany(c => c.Houses).HasForeignKey(h => h.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasData(SeedCategories());
        }
        private IEnumerable<House> SeedCategories()
        {
            return new House[]
            {
                new House
                {
                    Id = 1,
                    Title = "Jeremiah",
                    Address = "Nowhere",
                    Description = "Nothing",
                    ImageUrl = "",
                    PricePerMonth = 109.0M,
                    CategoryId = 2,
                    AgentId = 1
                },
                 new House
                {
                    Id = 1,
                    Title = "Nothing",
                    Address = "Nowhere",
                    Description = "Nothing",
                    ImageUrl = "",
                    PricePerMonth = 100.0M,
                    CategoryId = 2,
                    AgentId = 1
                },
            };
        }
    }
}
