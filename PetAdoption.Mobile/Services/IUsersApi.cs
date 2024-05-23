using Refit;

namespace PetAdoption.Mobile.Services
{
    [Headers("Authorization: Bearer")]
    public interface IUsersApi
    {
        [Put("/api/Users")]
        Task<ApiResponse> UpdateUserAsync(UserDto dto);

        [Get("/api/Users/{Id}")]
        Task<Shared.Dtos.ApiResponse<UserDto>> GetUserById(int Id);


        [Post("/api/Users/adopt/{petId}")]
        Task<ApiResponse> AdopPetAsync(int petId);

        [Get("/api/Users/adoptions")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetAdoptionPetList();

        [Get("/api/Users/favorites")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetFavoritePetList();

        [Post("/api/Users/favorite/{petId}")]
        Task<ApiResponse> ToggleFavoriteAsync(int petId);

        [Get("/api/Users/pet/{petId}")]
        Task<Shared.Dtos.ApiResponse<PetDetailDto>> GetPet(int petId);

        [Post("/api/Users/change-password")]
        Task<ApiResponse> ChangePasswordAsync(string newPw);

        [Post("/api/Message/Initialize")]
        Task<Shared.Dtos.ApiResponse<MessageInitializeReponse>> Initialize(MessageInitializeRequest request);

        [Post("/api/ListChat/Initialize")]
        Task<Shared.Dtos.ApiResponse<ListChatInitializeResponse>> InitializeFriend();

        [Get("/api/Users/owners")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetOwnersPetsAsync();
        [Post("/api/Users/create")]
        Task<ApiResponse> CreatePetAsync(PetCreateDto dto);

        [Put("/api/Users/update/{petId}")]
        Task<ApiResponse> UpdatePetAsync(int petId, PetUpdateDto dto);

        [Delete("/api/Users/delete/{petId}")]
        Task<ApiResponse> DeletePetAsync(int petId);
        [Get("/api/Users/pet")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetBySearch(string? input);
        [Get("/api/Users/pet/new/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetNewPetList(int count);
        [Get("/api/Users/pet/popular/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetPopularPetList(int count);
        [Get("/api/Users/pet/random/{count}")]
        Task<Shared.Dtos.ApiResponse<PetListDto[]>> GetRandomPetList(int count);
    }

}
