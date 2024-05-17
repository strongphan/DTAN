using Microsoft.AspNetCore.SignalR;
using PetAdoption.Shared.IChatHub;

namespace PetAdoption.Api.Hubs
{
    public class ChatHub : Hub, IChatServerHub
    {
        readonly IMessageService _messageService;
        private static readonly Dictionary<int, string> _connectionMapping = new();

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToUser(int fromUserId, int toUserId, string message)
        {
            var connectionIds = _connectionMapping.Where(x => x.Key == toUserId)
                                                    .Select(x => x.Value).ToList();

            await _messageService.AddMessageAsync(fromUserId, toUserId, message);

            await Clients.Clients(connectionIds)
                .SendAsync("ReceiveMessage", fromUserId, message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task Connect(int userId)
        {
            if (!_connectionMapping.ContainsKey(userId))
                _connectionMapping.Add(userId, Context.ConnectionId);
            return Task.CompletedTask;
        }

        public Task Disconnect(int userId)
        {
            _connectionMapping.Remove(userId);
            return Task.CompletedTask;
        }
    }
}
