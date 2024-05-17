namespace PetAdoption.Mobile.Services
{
    public class MessageInitializeReponse
    {
        public UserDto FriendInfo { get; set; }
        public IEnumerable<MessagesDto> Messages { get; set; }
    }
}
