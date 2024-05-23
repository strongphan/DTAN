namespace PetAdoption.Mobile.ViewModels
{
    public partial class AllPetsViewModel : BaseViewModel
    {
        private readonly IPetsApi _petsApi;
        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;

        public AllPetsViewModel(IPetsApi petsApi, AuthService authService, IUsersApi usersApi)
        {
            this._petsApi = petsApi;
            _authService = authService;
            _usersApi = usersApi;
        }

        [ObservableProperty]
        private IEnumerable<PetListDto> _pets = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isSearching;
        private bool _isInitialized;

        public async Task InitializeAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            await LoadAllPetsAsync(true);
        }
        private async Task LoadAllPetsAsync(bool initialLoad)
        {
            if (initialLoad)
                IsBusy = true;
            else
                IsRefreshing = true;
            try
            {
                ApiResponse<PetListDto[]> task;
                if (_authService.IsLoggedIn)
                {
                    task = await _usersApi.GetBySearch("");
                }
                else
                {
                    task = await _petsApi.GetAllPetList();

                }
                if (task.IsSuccess)
                {
                    Pets = task.Data;
                }
                else
                {
                    await ShowAlertAsync("Có lỗi", task.Msg);
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Có lỗi", ex.Message);
            }
            finally
            {
                IsBusy = IsRefreshing = false;
            }

        }

        [RelayCommand]
        private async Task LoadPets() => await LoadAllPetsAsync(false);
        [RelayCommand]
        private async Task Search(string input)
        {
            try
            {
                IsBusy = true;

                var task = await _petsApi.GetBySearch(input);

                if (task.IsSuccess)
                {
                    Pets = task.Data;
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Co loi: ", ex.Message);
            }
        }
    }
}
