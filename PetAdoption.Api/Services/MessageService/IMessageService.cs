using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IMessageService
    {
        Task<ApiResponse<int>> AddMessageAsync(int fromUserId, int toUserId, string message);
        Task<ApiResponse<LastestMessage[]>> GetLastestMessage(int userId);
        Task<ApiResponse<MessagesDto[]>> GetMessages(int fromUserId, int toUserId);
    }
}