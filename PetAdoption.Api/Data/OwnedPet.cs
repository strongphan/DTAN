namespace PetAdoption.Api.Data
{
    public partial class OwnedPet
    {
        public int Id { get; set; }

        public int PetId { get; set; }

        public int? UserId { get; set; }

        public virtual Pet Pet { get; set; } = null!;

        public virtual User? User { get; set; }
    }

}
