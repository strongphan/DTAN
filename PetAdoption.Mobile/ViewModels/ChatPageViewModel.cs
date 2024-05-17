using PetAdoption.Mobile.Hub;
using Refit;
using System.Collections.ObjectModel;

namespace PetAdoption.Mobile.ViewModels
{
    [QueryProperty(nameof(ReceiverId), nameof(ReceiverId))]
    [QueryProperty(nameof(SenderId), nameof(SenderId))]
    public partial class ChatPageViewModel : BaseViewModel, IAsyncDisposable
    {
        private readonly ChatHub _chatHub;

        private readonly IUsersApi _usersApi;

        [ObservableProperty]
        private int _senderId;

        [ObservableProperty]
        private int _receiverId;

        [ObservableProperty]
        private UserDto _friendInfo;

        [ObservableProperty]
        private ObservableCollection<MessagesDto> _messages;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private string _message;


        public ChatPageViewModel(ChatHub chatHub, IUsersApi usersApi)
        {
            _chatHub = chatHub;
            _usersApi = usersApi;
            _chatHub.AddReceivedMessageHandler(OnReceiveMessage);
        }
        private void OnReceiveMessage(int senderId, string message)
        {
            Messages.Add(new MessagesDto
            {
                Content = message,
                SenderId = ReceiverId,
                ReceiverId = SenderId,
                SentOn = DateTime.Now
            });

        }


        [RelayCommand]
        public async Task SendMessage()
        {
            try
            {
                if (Message.Trim() != "")
                {
                    await _chatHub.SendMessageToUser(SenderId, ReceiverId, Message);

                    Messages.Add(new MessagesDto
                    {
                        Content = Message,
                        SenderId = SenderId,
                        ReceiverId = ReceiverId,
                        SentOn = DateTime.Now
                    });

                    Message = "";

                }
            }
            catch (Exception ex)
            {
                await ShowAlertAsync("Có lỗi", ex.Message);
            }
        }


        async Task GetMessages()
        {
            var request = new MessageInitializeRequest
            {
                SenderId = SenderId,
                ReceiverId = ReceiverId,
            };

            var response = await _usersApi.Initialize(request);

            if (response.IsSuccess)
            {
                FriendInfo = response.Data.FriendInfo;
                Messages = new ObservableCollection<MessagesDto>(response.Data.Messages);
            }
            else
            {
                await ShowAlertAsync("Có lỗi", response.Msg);
            }
        }
        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                await _chatHub.Connect(SenderId);

                await GetMessages();
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



        [RelayCommand]
        private async Task GoBackAsync() => await GoToAsync("..");

        public async ValueTask DisposeAsync()
        {

            if (_chatHub is not null)
            {
                try
                {
                    await _chatHub.Disconnect(SenderId);

                }
                catch (Exception)
                {

                }
            }
        }
    }
}
