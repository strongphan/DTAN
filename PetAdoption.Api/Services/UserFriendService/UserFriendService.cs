using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public class UserFriendService : IUserFriendService
    {
        PetContext _petContext;
        IUserService _userService;
        public UserFriendService(PetContext petContext, IUserService userService)
        {
            _petContext = petContext;
            _userService = userService;
        }

        public async Task<ApiResponse> AddFriend(int userId, int friendId)
        {
            try
            {
                var existFriend = _petContext.UserFriends
                    .Where(f => (f.UserId == userId && f.FriendId == friendId) || (f.UserId == friendId && f.FriendId == userId));
                if (existFriend == null)
                {
                    UserFriend userFriend = new()
                    {
                        UserId = userId,
                        FriendId = friendId
                    };
                    _petContext.UserFriends.Add(userFriend);
                    await _petContext.SaveChangesAsync();
                }
                return ApiResponse.Success();
            }
            catch (Exception ex)
            {
                return ApiResponse.Fail(ex.Message);

            }

        }

        public async Task<ApiResponse<UserDto[]>> GetListUserFriend(int userId)
        {
            try
            {
                var entities = await _petContext.UserFriends.Where(x => x.UserId == userId || x.FriendId == userId).ToListAsync();

                var result = new List<UserDto>();

                foreach (var entity in entities)
                {
                    var res = await _userService.GetUserById(entity.FriendId);

                    if (userId == entity.FriendId)
                    {
                        res = await _userService.GetUserById(entity.UserId);

                    }
                    if (res != null)
                    {
                        result.Add(res.Data);
                    }
                }

                return ApiResponse<UserDto[]>.Success(result.ToArray());
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto[]>.Fail(ex.Message);
            }
        }
    }
}
