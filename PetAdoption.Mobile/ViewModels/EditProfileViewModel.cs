using System.Windows.Input;

namespace PetAdoption.Mobile.ViewModels
{
    public partial class EditProfileViewModel : BaseViewModel
    {
        private readonly IUsersApi _usersApi;
        private readonly AuthService _authService;

        public EditProfileViewModel(IUsersApi usersApi, AuthService authService)
        {
            _usersApi = usersApi;
            _authService = authService;
        }
        [ObservableProperty]
        private UserDto _model = new();
        public string Image
        {
            get { return Model.ProfilePicture; }
            set
            {
                if (Model.ProfilePicture != value)
                {
                    Model.ProfilePicture = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }
        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                if (!_authService.IsLoggedIn)
                {
                    await ShowToastAsync("Cần đăng nhập để xem!!");
                    return;
                }
                var user = _authService.GetUser();
                var userDto = await _usersApi.GetUserById(user.Id);
                Model = new UserDto
                {
                    Name = userDto.Data.Name,
                    Email = userDto.Data.Email,
                    Id = user.Id,
                    Address = userDto.Data.Address,
                    ProfilePicture = userDto.Data.ProfilePicture,
                    Password = userDto.Data.Password,
                    Phone = userDto.Data.Phone,

                };
                Image = userDto.Data.ProfilePicture;
                OnPropertyChanged(nameof(Image));
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
        private async Task UpdateAsync()
        {
            IsBusy = true;
            try
            {
                // Check if the image path is a local file path and read the file into a byte array
                if (!string.IsNullOrWhiteSpace(Model.ProfilePicture))
                {
                    try
                    {
                        if (Uri.TryCreate(Model.ProfilePicture, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                        {
                            using (var httpClient = new HttpClient())
                            {
                                var imageBytes = await httpClient.GetByteArrayAsync(Model.ProfilePicture);
                                Model.ImageData = imageBytes;
                            }
                        }
                        else // Assume it's a local file path
                        {
                            byte[] imageBytes = await File.ReadAllBytesAsync(Model.ProfilePicture);
                            Model.ImageData = imageBytes;
                        }
                    }
                    catch (Exception ex)
                    {
                        await ShowAlertAsync("Error reading image file", ex.Message);
                        IsBusy = false;
                        return;
                    }
                }
                var status = await _usersApi.UpdateUserAsync(Model);
                if (status.IsSuccess)
                {
                    await ShowToastAsync("Sửa thành công");
                    await GoToAsync("..");
                }
                else
                {
                    await ShowAlertAsync("Có lỗi", status.Msg);
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
        public ICommand SelectImageCommand => new Command(async () =>
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Chọn ảnh"
            });

            if (result != null)
            {
                Image = result.FullPath;
                Model.ProfilePicture = Image;
            }
        });
    }
}
