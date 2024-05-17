namespace PetAdoption.Shared.IChatHub
{
    public interface IChatClientHub
    {
        Task IsUserConnect(int userId);
        Task ReceiveMessage(int senderId, string message);
    }
}
