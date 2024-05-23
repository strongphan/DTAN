using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IPetService
    {
        Task<ApiResponse<List<PetListDto>>> GetAllPetList();
        Task<ApiResponse<List<PetListDto>>> GetBySearch(string? input);
        Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count);
        Task<ApiResponse<PetDetailDto>> GetPet(int id);
        Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count);
        Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count);
    }
}