namespace PetAdoption.Mobile.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly CommonService _commonService;
        private readonly IUsersApi _usersApi;

        public ProfileViewModel(AuthService authService, CommonService commonService, IUsersApi usersApi)
        {
            _authService = authService;
            _commonService = commonService;
            _usersApi = usersApi;
            _commonService.LoginStatusChanged += OnloginStatusChanged;
            SetUserInfo();
        }
        private void OnloginStatusChanged(object sender, EventArgs e) => SetUserInfo();

        private void SetUserInfo()
        {
            if (_authService.IsLoggedIn)
            {
                var userInfo = _authService.GetUser();
                UserName = userInfo.Name;
                ProfilePicture = userInfo.ProfilePicture;
                IsLoggedIn = true;
            }
            else
            {
                IsLoggedIn = false;
                UserName = "Khách";
                ProfilePicture = "";
            }
        }
        [ObservableProperty, NotifyPropertyChangedFor(nameof(Initials))]
        private string _userName = "Chưa đăng nhập";
        [ObservableProperty]
        private bool _isLoggedIn;
        [ObservableProperty]
        private string _profilePicture;
        public string Initials
        {
            get
            {
                var parts = UserName.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts.Length == 1)
                {
                    return UserName.Length == 1 ? UserName : UserName[..2];
                }
                return $"{parts[0][0]} {parts[1][0]}";
            }
        }
        [RelayCommand]
        private async Task LoginLogoutAsync()
        {
            if (!IsLoggedIn)
            {
                await GoToAsync($"//{nameof(LoginRegisterPage)}");
            }
            else
            {
                _authService.LogoutAsync();
                await GoToAsync($"//{nameof(HomePage)}");

            }
        }

        [RelayCommand]
        private async Task ChangePasswordAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowToastAsync("Cần đăng nhập!!");
                return;
            }
            else
            {
                var newPassword = await App.Current.MainPage.DisplayPromptAsync("Đổi mật khẩu", "Đổi mật khẩu", placeholder: "Nhập mật khẩu mới");
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    IsBusy = true;
                    await _usersApi.ChangePasswordAsync(newPassword);
                    IsBusy = false;
                    await ShowToastAsync("Đổi mật khẩu thành công");
                }
            }
        }
    }
}
