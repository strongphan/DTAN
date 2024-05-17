using Microsoft.AspNetCore.SignalR.Client;
using PetAdoption.Shared;
using PetAdoption.Shared.IChatHub;

namespace PetAdoption.Mobile.Hub
{
    public class ChatHub
    {
        private readonly HubConnection _hubConnection;
        private readonly List<Action<int, string>> _onReceiveMessageHandler;

        public ChatHub()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl(AppConstants.ChatHubFull).Build();
            _onReceiveMessageHandler = new List<Action<int, string>>();
            _hubConnection.On<int, string>("ReceiveMessage", OnReceiveMessage);

        }

        public async Task Connect(int userId)
        {
            await _hubConnection.StartAsync();
            await _hubConnection.SendAsync(nameof(IChatServerHub.Connect), userId);
        }

        public async Task Disconnect(int userId)
        {
            await _hubConnection.SendAsync(nameof(IChatServerHub.Disconnect), userId);
            await _hubConnection?.StopAsync();
        }

        public async Task SendMessageToUser(int senderId, int receiverId, string message)
        {
            await _hubConnection.InvokeAsync("SendMessageToUser", senderId, receiverId, message);
        }

        public void AddReceivedMessageHandler(Action<int, string> handler)
        {
            _onReceiveMessageHandler.Add(handler);
        }

        void OnReceiveMessage(int userId, string message)
        {
            foreach (var handler in _onReceiveMessageHandler)
            {
                handler(userId, message);
            }
        }

    }
}
