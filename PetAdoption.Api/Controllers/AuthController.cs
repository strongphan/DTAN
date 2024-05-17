using Microsoft.AspNetCore.Mvc;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ApiResponse<AuthResponseDto>> Login(LoginRequestDto dto) =>
            await _authService.LoginAsync(dto);

        [HttpPost("register")]
        public async Task<ApiResponse<AuthResponseDto>> Register(RegisterRequestDto dto) =>
            await _authService.RegisterAsync(dto);
    }
}
