using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Controllers
{
    public class ListChatInitializeResponse
    {
        public UserDto User { get; set; } = null!;
        public IEnumerable<UserDto> UserFriends { get; set; } = null!;
        public IEnumerable<LastestMessage> LastestMessages { get; set; } = null!;
    }
}
