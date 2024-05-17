namespace PetAdoption.Api.Data;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePicture { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<OwnedPet> OwnedPets { get; set; } = new List<OwnedPet>();

    public virtual ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();

    public virtual ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();

    public virtual ICollection<UserFriend> UserFriendFriends { get; set; } = new List<UserFriend>();

    public virtual ICollection<UserFriend> UserFriendUsers { get; set; } = new List<UserFriend>();
}
