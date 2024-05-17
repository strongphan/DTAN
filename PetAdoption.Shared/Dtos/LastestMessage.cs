namespace PetAdoption.Shared.Dtos
{
    public class LastestMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto UserFriendInfo { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
    }
}
