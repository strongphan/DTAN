using PetAdoption.Shared.Enumerations;

namespace PetAdoption.Shared.Dtos
{
    public class PetListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string Breed { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public double Distance { get; set; }
        public AdoptionStatus AdoptionStatus { get; set; }

    }
}
