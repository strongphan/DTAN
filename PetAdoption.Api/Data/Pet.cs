namespace PetAdoption.Api.Data;

public partial class Pet
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string? Type { get; set; }

    public string? Breed { get; set; }

    public int Gender { get; set; }

    public double Price { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Description { get; set; } = null!;

    public int Views { get; set; }

    public int AdoptionStatus { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<OwnedPet> OwnedPets { get; set; } = new List<OwnedPet>();

    public virtual ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();

    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
}
