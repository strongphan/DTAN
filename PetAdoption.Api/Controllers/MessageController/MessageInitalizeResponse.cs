using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Controllers
{
    public class MessageInitalizeResponse
    {
        public UserDto FriendInfo { get; set; } = null!;
        public IEnumerable<MessagesDto> Messages { get; set; } = null!;

    }
}
