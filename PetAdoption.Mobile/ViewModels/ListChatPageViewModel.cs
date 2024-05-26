using PetAdoption.Mobile.Hub;
using Refit;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PetAdoption.Mobile.ViewModels
{
    public partial class ListChatPageViewModel : BaseViewModel, IAsyncDisposable
    {



        private readonly ChatHub _chatHub;
        private readonly AuthService _authService;
        private readonly IUsersApi _usersApi;


        [ObservableProperty]
        private UserDto _userInfo;

        [ObservableProperty]
        private IEnumerable<UserDto> _userFriends = Enumerable.Empty<UserDto>();

        [ObservableProperty]
        private ObservableCollection<LastestMessage> _lastestMessages;

        [ObservableProperty]
        private bool _isRefreshing;


        public ICommand RefreshCommand { get; set; }

        public ICommand OpenChatPageCommand { get; set; }

        public ListChatPageViewModel(ChatHub chatHub, AuthService authService, IUsersApi usersApi)
        {
            _chatHub = chatHub;
            _authService = authService;
            _usersApi = usersApi;

            LastestMessages = new ObservableCollection<LastestMessage>();

            RefreshCommand = new Command(() =>
            {
                Task.Run(async () =>
                {
                    IsRefreshing = true;
                    await GetListFriends();
                }).GetAwaiter().OnCompleted(() =>
                {
                    IsRefreshing = false;
                });
            });

            OpenChatPageCommand = new Command<int>(async (param) =>
            {
                await Shell.Current.GoToAsync($"ChatPage?SenderId={UserInfo.Id}&ReceiverId={param}");
            });


            _chatHub.AddReceivedMessageHandler(OnReceivedMessage);
        }

        async Task GetListFriends()
        {
            var response = await _usersApi.InitializeFriend();

            if (response.IsSuccess)
            {
                UserInfo = response.Data.User;
                UserFriends = response.Data.UserFriends;

                LastestMessages = new ObservableCollection<LastestMessage>(response.Data.LastestMessages);
            }
            else
            {
                await ShowAlertAsync("Có lỗi", response.Msg);
            }
        }

        public async Task InitializeAsync()
        {
            if (!_authService.IsLoggedIn)
            {
                await ShowAlertAsync("Chưa đăng nhập", "Cần đăng nhập để xem!!");
                await GoToAsync($"//{nameof(HomePage)}");

                return;
            }

            var user = _authService.GetUser();
            UserInfo = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                ProfilePicture = user.ProfilePicture,
            };
            IsBusy = true;
            try
            {
                await _chatHub.Connect(UserInfo.Id);
                await GetListFriends();
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

        void OnReceivedMessage(int senderId, string message)
        {
            var lastestMessage = LastestMessages.Where(x => x.UserFriendInfo.Id == senderId).FirstOrDefault();
            if (lastestMessage != null)
                LastestMessages.Remove(lastestMessage);

            var newLastestMessage = new LastestMessage
            {
                UserId = UserInfo.Id,
                Content = message,
                UserFriendInfo = UserFriends.Where(x => x.Id == senderId).FirstOrDefault()
            };

            LastestMessages.Insert(0, newLastestMessage);
            OnPropertyChanged(nameof(LastestMessages));


        }
        public async ValueTask DisposeAsync()
        {
            if (_chatHub is not null)
            {
                try
                {
                    await _chatHub.Disconnect(UserInfo.Id);
                }
                catch (Exception)
                {

                }
            }

        }

    }

}
