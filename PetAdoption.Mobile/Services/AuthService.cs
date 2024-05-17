using Refit;

namespace PetAdoption.Mobile.Services
{
    public class AuthService
    {
        private readonly CommonService _commonService;
        private readonly IAuthApi _authApi;
        public bool IsLoggedIn => Preferences.Default.ContainsKey(UIConstants.UserInfo);

        public AuthService(CommonService commonService, IAuthApi authApi)
        {
            this._commonService = commonService;
            this._authApi = authApi;
        }
        public async Task<bool> LoginRegisterAsync(LoginRegisterModel model)
        {
            Shared.Dtos.ApiResponse<AuthResponseDto> apiResponse;

            try
            {
                if (model.IsNewUser)
                {
                    //Register Api
                    apiResponse = await _authApi.RegisterApi(new RegisterRequestDto
                    {
                        Email = model.Email,
                        Name = model.Name,
                        Password = model.Password,
                    });
                }
                else
                {
                    //Login Api
                    apiResponse = await _authApi.LoginApi(new LoginRequestDto
                    {
                        Email = model.Email,
                        Password = model.Password,
                    });
                }

            }
            catch (ApiException ex)
            {
                await App.Current.MainPage.DisplayAlert("Có lỗi", ex.Message, "Ok");
                return false;
            }
            if (!apiResponse.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Có lỗi", apiResponse.Msg, "Ok");
                return false;
            }
            var user = new LoggedInUser(apiResponse.Data.UserId, apiResponse.Data.Name, apiResponse.Data.ProfilePicture, apiResponse.Data.Address, apiResponse.Data.Token);
            _commonService.SetToken(apiResponse.Data.Token);
            _commonService.ToggleLoginStatus();
            SetUser(user);
            return true;
        }
        private void SetUser(LoggedInUser user) =>
                    Preferences.Default.Set(UIConstants.UserInfo, user.ToJson());

        public void LogoutAsync()
        {
            _commonService.SetToken(null);
            Preferences.Default.Remove(UIConstants.UserInfo);
            _commonService.ToggleLoginStatus();
        }
        public LoggedInUser GetUser()
        {
            var userJson = Preferences.Default.Get(UIConstants.UserInfo, string.Empty);
            return LoggedInUser.LoadFromJson(userJson);
        }
    }
}
