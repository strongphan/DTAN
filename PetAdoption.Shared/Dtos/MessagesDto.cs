namespace PetAdoption.Shared.Dtos
{
    public class MessagesDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
    }
}
