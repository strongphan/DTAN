namespace PetAdoption.Mobile.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {

        [ObservableProperty]
        private bool _isBusy;

        /*[RelayCommand]
        public async Task GoToChatPage(int userId, int receiverId) =>
            await GoToAsync($"{nameof(ChatPage)}?SenderId={userId}&ReceiverId={receiverId}");*/

        [RelayCommand]
        public async Task GoToDetailPage(int petId) =>
          await GoToAsync($"{nameof(DetailsPage)}?{nameof(DetailViewModel.PetId)}={petId}");

        public async Task GoToAsync(ShellNavigationState state) =>
            await Shell.Current.GoToAsync(state);

        public async Task GoToAsync(ShellNavigationState state, bool animate) =>
            await Shell.Current.GoToAsync(state, animate);

        public async Task GoToAsync(ShellNavigationState state, IDictionary<string, object> parameters)
            => await Shell.Current.GoToAsync(state, parameters);

        public async Task GoToAsync(ShellNavigationState state, bool animate, IDictionary<string, object> parameters) =>
            await Shell.Current.GoToAsync(state, animate, parameters);

        public async Task ShowToastAsync(string Message)
        {
            await Toast.Make(Message).Show();
        }
        public async Task ShowAlertAsync(string title, string msg)
        {
            await App.Current.MainPage.DisplayAlert(title, msg, "Đóng");
        }
        public async Task<bool> ShowConfirmAsync(string title, string msg) =>
            await App.Current.MainPage.DisplayAlert(title, msg, "Đồng ý", "Hủy bỏ");

    }
}
