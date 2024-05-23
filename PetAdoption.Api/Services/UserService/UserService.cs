using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public class UserService : IUserService
    {
        private readonly PetContext _petContext;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public UserService(PetContext petContext, IMapper mapper, IWebHostEnvironment env)
        {
            _petContext = petContext;
            _mapper = mapper;
            _env = env;
        }

        public async Task<ApiResponse> EditUser(int Id, UserDto dto)
        {
            try
            {
                var user = await _petContext.Users.FirstOrDefaultAsync(p => p.Id == Id);
                if (user == null)
                {
                    return ApiResponse.Fail("Không tìm thấy");
                }
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    var oldImagePath = Path.Combine(_env.WebRootPath, "images/avatars", user.ProfilePicture);
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }
                var imageID = Guid.NewGuid();

                if (dto.ImageData != null && dto.ImageData.Length > 0)
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "images/avatars", $"image_{imageID}.jpg");

                    await File.WriteAllBytesAsync(imagePath, dto.ImageData);
                }
                _mapper.Map(dto, user);

                user.ProfilePicture = $"image_{imageID}.jpg";

                _petContext.Users.Update(user);
                await _petContext.SaveChangesAsync();

                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserById(int id)
        {
            var user = await _petContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return ApiResponse<UserDto>.Fail("Không tồn tại người dùng");
            }
            UserDto userDto = _mapper.Map<UserDto>(user);
            return ApiResponse<UserDto>.Success(userDto);
        }
    }
}
