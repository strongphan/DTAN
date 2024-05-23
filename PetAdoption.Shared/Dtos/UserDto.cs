using System.ComponentModel.DataAnnotations;

namespace PetAdoption.Shared.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public string Email { get; set; }
        [Required]

        public string Password { get; set; }
        public byte[] ImageData { get; set; }
        [Required]
        public string ProfilePicture { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
