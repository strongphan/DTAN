namespace PetAdoption.Shared.IPetHub
{
    public interface IPetClientHub
    {
        Task PetIsBeingViewed(int petId);
        Task PetAdopted(int petId);
    }
}
