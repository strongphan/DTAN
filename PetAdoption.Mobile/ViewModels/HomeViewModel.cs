using Refit;

namespace PetAdoption.Mobile.ViewModels
{
    public partial class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(IPetsApi petsApi, IUsersApi usersApi, CommonService commonService, AuthService authService)
        {
            _petsApi = petsApi;
            _usersApi = usersApi;
            _commonService = commonService;
            _authService = authService;
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
                _commonService.SetToken(userInfo.Token);
            }
            else
            {
                UserName = "Khách";
            }
        }
        [ObservableProperty]
        private IEnumerable<PetListDto> _newlyAdded = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private IEnumerable<PetListDto> _popular = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private IEnumerable<PetListDto> _random = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private IEnumerable<PetListDto> _all = Enumerable.Empty<PetListDto>();

        [ObservableProperty]
        private string _userName = "Khách";

        private readonly IPetsApi _petsApi;
        private readonly IUsersApi _usersApi;
        private readonly CommonService _commonService;
        private readonly AuthService _authService;
        private bool _isInitialize;

        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                Task<Shared.Dtos.ApiResponse<PetListDto[]>> newlyTask;
                Task<Shared.Dtos.ApiResponse<PetListDto[]>> popularTask;
                Task<Shared.Dtos.ApiResponse<PetListDto[]>> randomTask;
                if (_authService.IsLoggedIn)
                {
                    newlyTask = _usersApi.GetNewPetList(10);
                    popularTask = _usersApi.GetPopularPetList(10);
                    randomTask = _usersApi.GetRandomPetList(10);
                }
                else
                {
                    newlyTask = _petsApi.GetNewPetList(10);
                    popularTask = _petsApi.GetPopularPetList(10);
                    randomTask = _petsApi.GetRandomPetList(10);
                }

                await Task.WhenAll(newlyTask, popularTask, randomTask);

                NewlyAdded = (await newlyTask).Data;
                Random = (await randomTask).Data;
                Popular = (await popularTask).Data;
                _isInitialize = true;

            }
            catch (ApiException ex)
            {
                await ShowAlertAsync("Có lỗi", ex.Message);
                return;
            }
            finally
            {
                IsBusy = false;
            }
        }



    }

}
