using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IUserFriendService
    {
        Task<ApiResponse<UserDto[]>> GetListUserFriend(int userId);
        Task<ApiResponse> AddFriend(int userId, int friendId);
    }
}