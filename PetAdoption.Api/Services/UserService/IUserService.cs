using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IUserService
    {
        Task<ApiResponse<UserDto>> GetUserById(int id);
    }
}