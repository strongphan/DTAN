using PetAdoption.Shared.Enumerations;

namespace PetAdoption.Shared.Dtos
{
    public class PetDetailDto : PetListDto
    {

        public bool isFavorite { get; set; }
        public bool IsActive { get; set; }
        public int Views { get; set; }
        public string? Description { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string GenderDisplay => Gender switch { Gender.Male => "Đực", Gender.Female => "Cái", _ => throw new NotImplementedException() };
        public string GenderImage => Gender switch { Gender.Male => "Male", Gender.Female => "Female", _ => throw new NotImplementedException() };
        public string Age
        {
            get
            {
                var diff = DateTime.Now.Subtract(DateOfBirth);
                var days = diff.Days;
                return days switch
                {
                    < 30 => days + " ngày",
                    >= 30 and <= 31 => "1 tháng",
                    < 365 => Math.Floor(diff.TotalDays / 30) + " tháng",
                    365 => "1 năm",
                    _ => Math.Floor(diff.TotalDays / 365) + " năm"
                };
            }
        }
        public UserDto Owner { get; set; }

    }
}
