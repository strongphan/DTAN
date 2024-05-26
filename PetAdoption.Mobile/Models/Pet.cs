using PetAdoption.Shared.Enumerations;

namespace PetAdoption.Mobile.Models
{
    public partial class Pet : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public string Breed { get; set; }
        public double Distance { get; set; }

        [ObservableProperty]
        private bool _isFavorite;

        [ObservableProperty]
        public AdoptionStatus _adoptionStatus;
        public string GenderDisplay { get; set; }
        public string GenderImage { get; set; }
        public string Age { get; set; }
        public UserDto Owner { get; set; } = null!;

    }
}
