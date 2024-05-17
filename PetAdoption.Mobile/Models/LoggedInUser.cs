using System.Text.Json;

namespace PetAdoption.Mobile.Models
{
    public record LoggedInUser(int Id, string Name, string ProfilePicture, string Address, string Token)
    {
        public string ToJson() =>
            JsonSerializer.Serialize(this);
        public static LoggedInUser LoadFromJson(string json) =>
            !string.IsNullOrWhiteSpace(json)
            ? JsonSerializer.Deserialize<LoggedInUser>(json)
            : default;
    };

}
