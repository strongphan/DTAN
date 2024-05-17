namespace PetAdoption.Shared.IChatHub
{
    public interface IChatServerHub
    {
        Task Connect(int userId);
        Task Disconnect(int userId);
    }
}
