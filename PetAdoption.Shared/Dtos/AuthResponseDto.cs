namespace PetAdoption.Shared.Dtos
{
    public record AuthResponseDto(int UserId, string Name, string ProfilePicture, string Address, string Token);
}
