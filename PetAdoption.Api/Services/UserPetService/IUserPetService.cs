using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IUserPetService
    {
        Task<ApiResponse> AdopPetAsync(int userId, int petId);
        Task<ApiResponse<List<PetListDto>>> GetAdoptionPetList(int userId);
        Task<ApiResponse<List<PetListDto>>> GetFavoritePetList(int userId);
        Task<ApiResponse> ToggleFavoriteAsync(int userId, int petId);
        Task<ApiResponse> CreatePet(int userId, PetCreateDto dto);
        Task<ApiResponse<List<PetListDto>>> GetOwnerPetList(int userId);
        Task<ApiResponse> UpdatePet(int petId, PetUpdateDto dto);
        Task<ApiResponse> DeletePet(int petId);
        Task<ApiResponse<List<PetListDto>>> GetAllPetList(string address);
        Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count, string address);
        Task<ApiResponse<PetDetailDto>> GetPet(int petId, int userId, string address);
        Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count, string address);
        Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count, string address);
    }
}