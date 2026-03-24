using HouseRentingSystem.Data.Data.Entities;
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
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasData(SeedAgents());
        }

        private IEnumerable<Agent> SeedAgents()
        {
            return new Agent[]
            {
               new Agent
               {
                   Id = 1,
                   PhoneNumber = "123123123123123123",
                   UserId = "Abdul123"
               },
               new Agent
               {
                   Id = 2,
                   PhoneNumber = "253834738329248243944",
                   UserId = "Osama9/11"
               }

            };

        }
    }
}
