using PetAdoption.Shared.Enumerations;
using System.Windows.Input;

namespace PetAdoption.Mobile.ViewModels
{
    [QueryProperty(nameof(PetId), nameof(PetId))]
    public partial class UpdatePetViewModel : BaseViewModel
    {
        private readonly IUsersApi _usersApi;
        private readonly IPetsApi _petsApi;

        public UpdatePetViewModel(IUsersApi usersApi, IPetsApi petsApi)
        {
            _usersApi = usersApi;
            _petsApi = petsApi;
        }
        [ObservableProperty]
        private int _petId;
        [ObservableProperty]
        private PetUpdateDto _model = new();
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
        private int _selectedGender;
        public int SelectedGender
        {
            get { return _selectedGender; }
            set
            {
                _selectedGender = value;
                Model.Gender = _selectedGender;
                OnPropertyChanged(nameof(SelectedGender));
            }
        }
        public string AdoptionStatus { get; set; }
        public List<string> AdoptionStatuses
        {
            get
            {
                return Enum.GetValues(typeof(AdoptionStatus)).Cast<AdoptionStatus>().Select(status =>
                {
                    switch (status)
                    {
                        case PetAdoption.Shared.Enumerations.AdoptionStatus.Available:
                            return "Chưa được nhận";
                        case PetAdoption.Shared.Enumerations.AdoptionStatus.InProgress:
                            return "Đang xử lý";
                        case PetAdoption.Shared.Enumerations.AdoptionStatus.Adopted:
                            return "Đã được nhận";
                        default:
                            return status.ToString();
                    }
                }).ToList();
            }
        }
        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {

                var petDto = await _petsApi.GetPet(PetId);
                Model = new PetUpdateDto
                {
                    Name = petDto.Data.Name,
                    Type = petDto.Data.Type,
                    Image = petDto.Data.Image,
                    Breed = petDto.Data.Breed,
                    Views = petDto.Data.Views,
                    Gender = (int)petDto.Data.Gender,
                    Price = petDto.Data.Price,
                    DateOfBirth = petDto.Data.DateOfBirth,
                    Description = petDto.Data.Description,
                    AdoptionStatus = (int)petDto.Data.AdoptionStatus,
                    IsActive = petDto.Data.IsActive,
                };
                Image = petDto.Data.Image;
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
                var status = await _usersApi.UpdatePetAsync(PetId, Model);
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
                Model.Image = Image;
            }
        });

    }
}
