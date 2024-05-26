using System.Collections.ObjectModel;

namespace PetAdoption.Mobile.ViewModels
{
    public partial class FavoriteViewModel : BaseViewModel
    {
        private readonly IUsersApi _usersApi;
        private readonly AuthService _authService;

        public FavoriteViewModel(IUsersApi usersApi, AuthService authService
            )
        {
            _usersApi = usersApi;
            _authService = authService;
        }
        [ObservableProperty]
        public ObservableCollection<PetSlim> _pets = new ObservableCollection<PetSlim>();
        public async Task InitializeAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Chưa đăng nhập", "Cần đăng nhập để xem!!");
                await GoToAsync($"//{nameof(HomePage)}");
                return;
            }
            try
            {
                IsBusy = true;
                var res = await _usersApi.GetFavoritePetList();
                if (!res.IsSuccess)
                {
                    await ShowAlertAsync("Có lỗi", res.Msg);
                }
                else
                {
                    Pets = new ObservableCollection<PetSlim>(
                        res.Data.Select(p => new PetSlim
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Image = p.Image,
                            Price = p.Price,
                            IsFavorite = true
                        }));
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
        private async Task ToggleFavorite(int petId)
        {

            try
            {
                var pet = Pets.FirstOrDefault(Pets => Pets.Id == petId);
                if (pet != null)
                {
                    pet.IsFavorite = false;
                    IsBusy = true;
                    await _usersApi.ToggleFavoriteAsync(petId);
                    Pets.Remove(pet);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlertAsync("Có lỗi xảy ra", ex.Message);
            }
        }
    }
}
