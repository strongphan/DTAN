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

        public UserService(PetContext petContext, IMapper mapper)
        {
            _petContext = petContext;
            _mapper = mapper;
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
