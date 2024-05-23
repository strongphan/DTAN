using System.ComponentModel.DataAnnotations;

namespace PetAdoption.Shared.Dtos
{
    public class PetCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Image { get; set; } = null!;

        public byte[] ImageData { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Breed { get; set; }

        [Required]
        public int Gender { get; set; }
        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Description { get; set; } = null!;

        public int Views { get; set; } = 0!;

        public int AdoptionStatus { get; set; } = 0!;

        public bool IsActive { get; set; } = true;
    }
}
