﻿namespace PetAdoption.Shared.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
