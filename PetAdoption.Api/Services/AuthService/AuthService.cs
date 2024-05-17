using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly PetContext _context;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthService(PetContext context, TokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequestDto.Email);
            if (dbUser is null)
            {
                return ApiResponse<AuthResponseDto>.Fail("Không tồn tại tài khoản");
            }
            if (dbUser.Password != loginRequestDto.Password)
            {
                return ApiResponse<AuthResponseDto>.Fail("Sai tài khoản hoặc mật khẩu");
            }
            var token = _tokenService.GenerateJWT(dbUser);
            var userDto = _mapper.Map<UserDto>(dbUser);
            return ApiResponse<AuthResponseDto>.Success(new AuthResponseDto(userDto.Id, userDto.Name, userDto.ProfilePicture, userDto.Address, token));
        }
        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user is not null)
            {
                return ApiResponse<AuthResponseDto>.Fail("Đã tồn tại tài khoản");
            }
            var dbUser = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password,
                ProfilePicture = user.ProfilePicture,
                Address = user.Address,
            };
            await _context.Users.AddAsync(dbUser);
            await _context.SaveChangesAsync();
            var token = _tokenService.GenerateJWT(dbUser);
            return ApiResponse<AuthResponseDto>.Success(new AuthResponseDto(dbUser.Id, dbUser.Name, dbUser.ProfilePicture, dbUser.Address, token));
        }
        public async Task<ApiResponse> ChangePasswordAsync(int userId, string newPw)
        {
            var dbUser = await _context.Users.AsTracking().FirstOrDefaultAsync(p => p.Id == userId);

            if (dbUser is not null)
            {
                dbUser.Password = newPw;
                await _context.SaveChangesAsync();
                return ApiResponse.Success();
            }
            else
            {
                return ApiResponse.Fail("Có lỗi xảy ra, vui lòng thử lại!!");

            }
        }

    }
}
