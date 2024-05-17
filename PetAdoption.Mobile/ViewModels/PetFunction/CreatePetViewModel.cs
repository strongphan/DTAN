using System.Windows.Input;
namespace PetAdoption.Mobile.ViewModels
{
    public partial class CreatePetViewModel : BaseViewModel
    {
        private readonly IUsersApi _usersApi;

        public CreatePetViewModel(IUsersApi usersApi)
        {
            _usersApi = usersApi;
        }
        [ObservableProperty]
        private PetCreateDto _model = new();
        public string Image
        {
            get { return Model.Image; }
            set
            {
                if (Model.Image != value)
                {
                    Model.Image = value;
                    OnPropertyChanged(nameof(Image));
                }
            }
        }
        [RelayCommand]
        private async Task CreateAsync()
        {
            IsBusy = true;
            try
            {
                // Check if the image path is a local file path and read the file into a byte array
                if (!string.IsNullOrWhiteSpace(Model.Image))
                {
                    try
                    {
                        if (Uri.TryCreate(Model.Image, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                        {
                            using (var httpClient = new HttpClient())
                            {
                                var imageBytes = await httpClient.GetByteArrayAsync(Model.Image);
                                Model.ImageData = imageBytes;
                            }
                        }
                        else // Assume it's a local file path
                        {
                            byte[] imageBytes = await File.ReadAllBytesAsync(Model.Image);
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
                var status = await _usersApi.CreatePetAsync(Model);
                if (status.IsSuccess)
                {
                    await ShowToastAsync("Thêm thành công");
                    await GoToAsync("..");
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
                PickerTitle = "Select an image"
            });

            if (result != null)
            {
                Image = result.FullPath;
                Model.Image = result.FullPath;
            }
        });
    }
}
