namespace PetAdoption.Shared.IPetHub
{
    public interface IPetServerHub
    {
        Task ViewingThisPet(int petId);
        Task ReleaseViewingThisPet(int petId);
        Task PetAdopted(int petId);

    }
}
