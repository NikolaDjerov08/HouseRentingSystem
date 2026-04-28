using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HouseRentingSystem.Data.Data.Entities;

namespace HouseRentingSystem.Data.Entities
{
    public class House
    {
        public const int TitleMaxLength = 50;
        public const int AddressMaxLength = 150;
        public const int DescriptionMaxLength = 500;

        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Column(TypeName = "decimal(12,3)")]
        public decimal PricePerMonth { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; init; } = null!;

        public string AgentId { get; set; }
        public ApplicationUser Agent { get; init; } = null!;

        public string? RenterId { get; set; }
        public ApplicationUser? Renter { get; set; }
        public bool IsDeleted { get; set; }
    }
}
