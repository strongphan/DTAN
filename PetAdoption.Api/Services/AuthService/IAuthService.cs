using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto);
        Task<ApiResponse> ChangePasswordAsync(int UserId, string newPw);
    }
}