using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetAdoption.Shared.Dtos;
namespace PetAdoption.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        [HttpPost("Initialize")]
        public async Task<ApiResponse<MessageInitalizeResponse>> Initialize([FromBody] MessageInitalizeRequest request)
        {
            var response = new MessageInitalizeResponse
            {
                FriendInfo = _userService.GetUserById(request.ReceiverId).Result.Data,
                Messages = _messageService.GetMessages(request.SenderId, request.ReceiverId).Result.Data.ToList()
            };

            return ApiResponse<MessageInitalizeResponse>.Success(response);
        }
    }
}
