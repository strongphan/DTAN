using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdoption.Shared.Dtos;
using System.Security.Claims;
namespace PetAdoption.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class ListChatController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserFriendService _userFriendService;
        private readonly IMessageService _messageService;
        private int UserId => Convert.ToInt32(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ListChatController(IUserService userService, IUserFriendService userFriendService, IMessageService messageService)
        {
            _userService = userService;
            _userFriendService = userFriendService;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<ApiResponse<LastestMessage[]>> GetLastestMessage(int id) =>
               await _messageService.GetLastestMessage(id);


        [HttpPost("Initialize")]
        public async Task<ApiResponse<ListChatInitializeResponse>> Initialize()
        {

            var response = new ListChatInitializeResponse
            {
                User = _userService.GetUserById(UserId).Result.Data,
                UserFriends = _userFriendService.GetListUserFriend(UserId).Result.Data,
                LastestMessages = _messageService.GetLastestMessage(UserId).Result.Data
            };

            return ApiResponse<ListChatInitializeResponse>.Success(response);
        }
    }
}
