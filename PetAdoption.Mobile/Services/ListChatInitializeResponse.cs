namespace PetAdoption.Mobile.Services
{
    public class ListChatInitializeResponse
    {
        public UserDto User { get; set; }
        public IEnumerable<UserDto> UserFriends { get; set; }
        public IEnumerable<LastestMessage> LastestMessages { get; set; }
    }
}
