namespace PetAdoption.Shared.Dtos
{
    public class PetUpdateDto
    {
        public string Name { get; set; }

        public string Image { get; set; }
        public byte[] ImageData { get; set; }

        public string? Type { get; set; }

        public string? Breed { get; set; }

        public int Gender { get; set; }
        public double Price { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public int Views { get; set; }
        public int AdoptionStatus { get; set; }

        public bool IsActive { get; set; }
    }
}
