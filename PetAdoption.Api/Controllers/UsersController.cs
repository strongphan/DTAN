using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdoption.Shared.Dtos;
using System.Security.Claims;

namespace PetAdoption.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserPetService _userPetService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserFriendService _userFriendService;

        private int UserId => Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        private string UserAddress => Convert.ToString(User.Claims.First(c => c.Type == ClaimTypes.StreetAddress).Value);
        public UsersController(
            IUserPetService userPetService, IAuthService authService, IUserService userService, IUserFriendService userFriendService)
        {
            _userPetService = userPetService;
            _authService = authService;
            _userService = userService;
            _userFriendService = userFriendService;
        }
        [HttpPost("friendId")]
        public async Task<ApiResponse> AddFriend(int friendId) =>
            await _userFriendService.AddFriend(UserId, friendId);

        [HttpGet("{Id}")]
        public async Task<ApiResponse<UserDto>> GetUserById(int Id) =>
            await _userService.GetUserById(Id);
        [HttpPost("adopt/{petId}")]
        public async Task<ApiResponse> AdopPetAsync(int petId) =>
            await _userPetService.AdopPetAsync(UserId, petId);

        [HttpGet("adoptions")]
        public async Task<ApiResponse<List<PetListDto>>> GetAdoptionPetList() =>
            await _userPetService.GetAdoptionPetList(UserId);

        [HttpGet("favorites")]
        public async Task<ApiResponse<List<PetListDto>>> GetFavoritePetList() =>
            await _userPetService.GetFavoritePetList(UserId);

        [HttpPost("favorite/{petId}")]
        public async Task<ApiResponse> ToggleFavoriteAsync(int petId) =>
            await _userPetService.ToggleFavoriteAsync(UserId, petId);

        [HttpPut("")]
        public async Task<ApiResponse> UpdateUserAsync(UserDto dto) =>
           await _userService.EditUser(UserId, dto);

        [HttpGet("pet/{petId}")]
        public async Task<ApiResponse<PetDetailDto>> GetPet(int petId) =>
            await _userPetService.GetPet(petId, UserId, UserAddress);


        [HttpPost("change-password")]
        public async Task<ApiResponse> ChangePassword(string newPw) =>
            await _authService.ChangePasswordAsync(UserId, newPw);

        [HttpGet("owners")]
        public async Task<ApiResponse<List<PetListDto>>> GetOwnersPetsAsync() =>
            await _userPetService.GetOwnerPetList(UserId);

        [HttpPost("create")]
        public async Task<ApiResponse> CreatePetAsync(PetCreateDto dto) =>
            await _userPetService.CreatePet(UserId, dto);

        [HttpPut("update/{petId}")]
        public async Task<ApiResponse> UpdatePetAsync(int petId, PetUpdateDto dto) =>
            await _userPetService.UpdatePet(petId, dto);

        [HttpDelete("pet/delete/{petId}")]
        public async Task<ApiResponse> DeletePetAsync(int petId) =>
            await _userPetService.DeletePet(petId);
        [HttpGet("pet")]
        public async Task<ApiResponse<List<PetListDto>>> GetBySearch(string? input) =>
            await _userPetService.GetBySearch(input, UserId);

        [HttpGet("pet/new/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetNewPetList(int count) =>
            await _userPetService.GetNewPetList(count, UserId, UserAddress);

        [HttpGet("pet/popular/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetPopularPetList(int count) =>
            await _userPetService.GetPopularPetList(count, UserId, UserAddress);

        [HttpGet("pet/random/{count:int}")]
        public async Task<ApiResponse<List<PetListDto>>> GetRandomPetList(int count) =>
            await _userPetService.GetRandomPetList(count, UserId, UserAddress);
    }
}
