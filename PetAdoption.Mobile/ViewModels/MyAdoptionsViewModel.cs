namespace PetAdoption.Mobile.ViewModels
{
    public partial class MyAdoptionsViewModel : BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;

        public MyAdoptionsViewModel(AuthService authService, IUsersApi usersApi)
        {
            _authService = authService;
            _usersApi = usersApi;
        }
        [ObservableProperty]
        private IEnumerable<PetListDto> _myAdoption = Enumerable.Empty<PetListDto>();
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
                var res = await _usersApi.GetAdoptionPetList();
                if (!res.IsSuccess)
                {
                    await ShowAlertAsync("Có lỗi", res.Msg);
                }
                else
                {
                    MyAdoption = res.Data;
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
    }
}
