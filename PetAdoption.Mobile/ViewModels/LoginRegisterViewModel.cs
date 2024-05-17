namespace PetAdoption.Mobile.ViewModels
{
    [QueryProperty(nameof(IsFirstTime), nameof(IsFirstTime))]
    public partial class LoginRegisterViewModel : BaseViewModel
    {
        public LoginRegisterViewModel(AuthService authService)
        {
            this._authService = authService;
        }

        [ObservableProperty]
        private bool _isRegisterMode;

        [ObservableProperty]
        private LoginRegisterModel _model = new();

        [ObservableProperty]
        private bool _isFirstTime;
        private readonly AuthService _authService;

        partial void OnIsFirstTimeChanging(bool value)
        {
            if (value)
                IsRegisterMode = true;
        }
        [RelayCommand]
        private void ToggleMode() => IsRegisterMode = !IsRegisterMode;

        [RelayCommand]
        private async Task SkipNow() =>
            await GoToAsync($"//{nameof(HomePage)}");

        [RelayCommand]
        private async Task Submit()
        {
            if (!Model.Validate(IsRegisterMode))
            {
                await ShowToastAsync("Mời nhập đầy đủ thông tin");
                return;
            }
            IsBusy = true;
            var status = await _authService.LoginRegisterAsync(Model);
            if (status)
            {
                await SkipNow();
            }
            IsBusy = false;

        }


    }
}
