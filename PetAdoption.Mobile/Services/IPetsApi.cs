using Refit;

namespace PetAdoption.Mobile.Services
{
    public interface IPetsApi
    {
        [Get("/api/Pets")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetAllPetList();

        [Get("/api/Pets/filter")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetBySearch(string input);
        [Get("/api/Pets/new/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetNewPetList(int count);

        [Get("/api/Pets/popular/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetPopularPetList(int count);

        [Get("/api/Pets/random/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetRandomPetList(int count);

        [Get("/api/Pets/{id}")]
        Task<Shared.Dtos.ApiResponse<PetDetailDto>> GetPet(int id);
    }
}
