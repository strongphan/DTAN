using Microsoft.AspNetCore.SignalR.Client;
using PetAdoption.Shared;
using PetAdoption.Shared.Enumerations;
using PetAdoption.Shared.IPetHub;

namespace PetAdoption.Mobile.ViewModels
{
    [QueryProperty(nameof(PetId), nameof(PetId))]
    public partial class DetailViewModel : BaseViewModel, IAsyncDisposable
    {
        public DetailViewModel(IPetsApi petsApi, AuthService authService, IUsersApi usersApi)
        {
            this._petsApi = petsApi;
            _authService = authService;
            _usersApi = usersApi;
        }
        [ObservableProperty]
        private int _petId;

        [ObservableProperty]
        private Pet _petDetail = new();

        private readonly IPetsApi _petsApi;
        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;
        private HubConnection _connection;
        private async Task ConfigureSignalRHubConnectionAsync(int currentPetId)
        {
            try
            {
                _connection = new HubConnectionBuilder().WithUrl(AppConstants.PetHubFull).Build();
                _connection.On<int>(nameof(IPetClientHub.PetIsBeingViewed), async petId =>
                {
                    if (currentPetId == PetId)
                    {
                        await App.Current.Dispatcher.DispatchAsync(() => ShowToastAsync("Có ai đó cũng đang xem thú cưng này"));

                    }
                });
                _connection.On<int>(nameof(IPetClientHub.PetAdopted), async petId =>
                {
                    if (currentPetId == PetId)
                    {
                        PetDetail.AdoptionStatus = AdoptionStatus.Adopted;
                        await App.Current.Dispatcher.DispatchAsync(() => ShowToastAsync("Có ai đó đã nhận nuôi thú cưng này"));

                    }
                });
                await _connection.StartAsync();
                await _connection.SendAsync(nameof(IPetServerHub.ViewingThisPet), currentPetId);
            }
            catch (Exception ex)
            {
                await ShowToastAsync(ex.Message);

            }
        }
        async partial void OnPetIdChanging(int petId)
        {
            IsBusy = true;
            try
            {
                await ConfigureSignalRHubConnectionAsync(petId);
                var res = _authService.IsLoggedIn
                    ? await _usersApi.GetPet(petId)
                    : await _petsApi.GetPet(petId);

                if (res.IsSuccess)
                {
                    var pet = res.Data;
                    PetDetail = new Pet
                    {
                        AdoptionStatus = pet.AdoptionStatus,
                        Age = pet.Age,
                        Breed = pet.Breed,
                        Description = pet.Description,
                        GenderDisplay = pet.GenderDisplay,
                        GenderImage = pet.GenderImage,
                        Name = pet.Name,
                        Price = pet.Price,
                        IsFavorite = pet.isFavorite,
                        Image = pet.Image,
                        Owner = pet.Owner,
                        distance = pet.Distance,
                    };
                }
                else
                {
                    await ShowAlertAsync("Có lỗi:", res.Msg);

                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Có lỗi:", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoBackAsync() => await GoToAsync("..");

        [RelayCommand]
        private async Task ToggleFavorite()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowToastAsync("Cần đăng nhập để thực hiện");
                return;
            }
            PetDetail.IsFavorite = !PetDetail.IsFavorite;
            try
            {
                IsBusy = true;
                await _usersApi.ToggleFavoriteAsync(PetId);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                PetDetail.IsFavorite = !PetDetail.IsFavorite;
                await ShowAlertAsync("Có lỗi xảy ra", ex.Message);
            }
        }

        [RelayCommand]
        private async Task AdoptNowAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                var result = await ShowConfirmAsync("Chưa đăng nhập", "Cần đăng nhập để nhận nuôi. " + Environment.NewLine + "Bạn có muốn đi đến đăng nhập");
                if (result)
                {
                    await GoToAsync($"//{nameof(LoginRegisterPage)}");
                }
                else
                {
                    return;
                }
            }
            IsBusy = true;
            try
            {
                var res = await _usersApi.AdopPetAsync(PetId);
                if (res.IsSuccess)
                {
                    PetDetail.AdoptionStatus = AdoptionStatus.Adopted;
                    if (_connection is not null)
                    {
                        try
                        {
                            await _connection.SendAsync(nameof(IPetServerHub.PetAdopted), PetId);
                        }
                        catch
                        {
                        }
                    }
                    await GoToAsync(nameof(AdoptionSuccessPage));

                }
                else
                {
                    await ShowAlertAsync("Có lỗi", res.Msg);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ShowAlertAsync("Có lỗi", ex.Message);

            }
        }

        public async ValueTask DisposeAsync()
        {

            if (_connection is not null)
            {
                try
                {
                    await _connection.SendAsync(nameof(IPetServerHub.ReleaseViewingThisPet), PetId);
                    await _connection.StopAsync();
                }
                catch (Exception)
                {

                }
            }
        }

    }
    public class DirectionsResult
    {
        public Route[] routes { get; set; }
    }
    public class RouteResponse
    {
        public double TravelDistance { get; set; }
        // Add other relevant properties if needed
    }
    public class Route
    {
        public Leg[] legs { get; set; }
    }

    public class Leg
    {
        public Distance distance { get; set; }
    }

    public class Distance
    {
        public double value { get; set; }
    }
}
