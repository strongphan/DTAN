namespace PetAdoption.Mobile.ViewModels
{
    public partial class AllPetsViewModel : BaseViewModel
    {
        private readonly IPetsApi _petsApi;

        public AllPetsViewModel(IPetsApi petsApi)
        {
            this._petsApi = petsApi;
        }

        [ObservableProperty]
        private IEnumerable<PetListDto> _pets = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private bool _isRefreshing;

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
                var task = _petsApi.GetAllPetList();
                await Task.WhenAll(task);
                var res = task.Result;
                if (res.IsSuccess)
                {
                    Pets = res.Data;
                }
                else
                {
                    await ShowAlertAsync("Có lỗi", res.Msg);
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
    }
}
