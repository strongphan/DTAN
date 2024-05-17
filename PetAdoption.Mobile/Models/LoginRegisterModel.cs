namespace PetAdoption.Mobile.Models
{
    public partial class LoginRegisterModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        public bool IsNewUser => !string.IsNullOrWhiteSpace(Name);
        public bool Validate(bool isRegisterMode)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }
            if (isRegisterMode && string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            return true;
        }
    }
}
