using Microsoft.AspNetCore.SignalR.Client;
using PetAdoption.Mobile.Hub;
using PetAdoption.Shared;
using PetAdoption.Shared.Enumerations;
using PetAdoption.Shared.IPetHub;

namespace PetAdoption.Mobile.ViewModels
{
    [QueryProperty(nameof(PetId), nameof(PetId))]
    public partial class DetailViewModel : BaseViewModel, IAsyncDisposable
    {
        public DetailViewModel(IPetsApi petsApi, AuthService authService, IUsersApi usersApi, ChatHub chatHub)
        {
            this._petsApi = petsApi;
            _authService = authService;
            _usersApi = usersApi;
            _chatHub = chatHub;
        }
        [ObservableProperty]
        private int _petId;

        [ObservableProperty]
        private bool _isLoggedIn;
        [ObservableProperty]
        private Pet _petDetail = new();

        private readonly IPetsApi _petsApi;
        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;
        private readonly ChatHub _chatHub;
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
                IsLoggedIn = _authService.IsLoggedIn;
                var res = IsLoggedIn
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
                        Distance = pet.Distance,
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
        private async Task GotoChatPage(int param)
        {
            if (!_authService.IsLoggedIn)
            {
                var result = await ShowConfirmAsync("Chưa đăng nhập", "Cần đăng nhập để nhận nuôi. " + Environment.NewLine + "Bạn có muốn đi đến đăng nhập");
                if (result)
                {
                    await GoToAsync($"//{nameof(LoginRegisterPage)}");
                    return;
                }
                else
                {
                    return;
                }
            }
            var userId = _authService.GetUser().Id;

            await Shell.Current.GoToAsync($"ChatPage?SenderId={userId}&ReceiverId={param}");

        }
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
            var confirm = await ShowConfirmAsync("Xác nhận", "Bạn có chắc chắn muốn nhận nuôi thú cưng này?");

            if (confirm)
            {
                if (!_authService.IsLoggedIn)
                {
                    var result = await ShowConfirmAsync("Chưa đăng nhập", "Cần đăng nhập để nhận nuôi. " + Environment.NewLine + "Bạn có muốn đi đến đăng nhập");
                    if (result)
                    {
                        await GoToAsync($"//{nameof(LoginRegisterPage)}");
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                IsBusy = true;
                try
                {
                    var userId = _authService.GetUser().Id;
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
                        await _chatHub.Connect(userId);
                        await _chatHub.SendMessageToUser(userId, PetDetail.Owner.Id, "Tôi đã nhận nuôi thú cưng của bạn, vui lòng liên hệ để giao dịch!!");
                        await _chatHub.Disconnect(userId);
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
            else
            {
                return;
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

}
