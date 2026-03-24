using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HouseRentingSystem.Data.Data.DataConstants.House;

namespace HouseRenting.Data
{
    public class Class1
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [MinLength(10)]
        [Required]
        public string Title { get; set; }
        [MaxLength(150)]
        [MinLength(30)]
        [Required]
        public string Address { get; set; }
        [MaxLength(500)]
        [MinLength(50)]
        [Required]
        public string Describtion { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        [Column(TypeName = "decimal(12, 3")]
        public decimal PricePerMonth { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; init; } = null;
        public int AgentId { get; set; }
        public Agent Agent { get; set; }
        public string? RenterId { get; set; }
        public IdentityUser? Renter { get; set; }
    }
}
