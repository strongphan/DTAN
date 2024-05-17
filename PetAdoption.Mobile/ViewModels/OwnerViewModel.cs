namespace PetAdoption.Mobile.ViewModels
{
    public partial class OwnerViewModel : BaseViewModel
    {

        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;

        public OwnerViewModel(AuthService authService, IUsersApi usersApi)
        {
            _authService = authService;
            _usersApi = usersApi;
        }
        [ObservableProperty]
        private IEnumerable<PetListDto> _myPets = Enumerable.Empty<PetListDto>();
        public async Task InitializeAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowToastAsync("Cần đăng nhập để xem!!");
                return;
            }
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                var res = await _usersApi.GetOwnersPetsAsync();
                if (!res.IsSuccess)
                {
                    await ShowAlertAsync("Có lỗi", res.Msg);
                }
                else
                {
                    MyPets = res.Data;
                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Có lỗi", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DeleteAsync(int petId)
        {
            IsBusy = true;
            var status = await _usersApi.DeletePetAsync(petId);
            if (status.IsSuccess)
            {
                await ShowToastAsync("Xóa thành công");
            }
            IsBusy = false;
        }
        [RelayCommand]
        public async Task GoToUpdate(int petId)
        {
            await GoToAsync($"{nameof(UpdatePetPage)}?{nameof(UpdatePetViewModel.PetId)}={petId}");
        }

        [RelayCommand]
        public async Task GoToCreate()
        {
            await GoToAsync($"{nameof(CreatePetPage)}");
        }
    }
}
