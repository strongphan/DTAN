using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetAdoption.Api.Data;
using PetAdoption.Shared.Dtos;

namespace PetAdoption.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly PetContext _petContext;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MessageService(PetContext petContext, IUserService userService, IMapper mapper)
        {
            _petContext = petContext;
            _userService = userService;
            _mapper = mapper;
        }
        public async Task<ApiResponse<int>> AddMessageAsync(int fromUserId, int toUserId, string message)
        {
            try
            {
                var entity = new Message
                {
                    SenderId = fromUserId,
                    ReceiverId = toUserId,
                    Content = message,
                    SentOn = DateTime.Now,
                    IsRead = false
                };

                _petContext.Messages.Add(entity);
                var result = await _petContext.SaveChangesAsync();

                return ApiResponse<int>.Success(result);
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);

            }
        }

        public async Task<ApiResponse<LastestMessage[]>> GetLastestMessage(int userId)
        {
            try
            {
                var result = new List<LastestMessage>();

                var userFriends = await _petContext.UserFriends.Where(x => x.UserId == userId || x.FriendId == userId).ToListAsync();

                foreach (var userFriend in userFriends)
                {
                    var lastMessage = await _petContext.Messages.Where(x => (x.SenderId == userId && x.ReceiverId == userFriend.FriendId)
                                 || (x.SenderId == userFriend.FriendId && x.ReceiverId == userId))
                        .OrderByDescending(x => x.SentOn)
                        .FirstOrDefaultAsync();

                    if (userId == userFriend.FriendId)
                    {
                        lastMessage = await _petContext.Messages.Where(x => (x.SenderId == userId && x.ReceiverId == userFriend.UserId)
                                 || (x.SenderId == userFriend.UserId && x.ReceiverId == userId))
                        .OrderByDescending(x => x.SentOn)
                        .FirstOrDefaultAsync();

                    }
                    if (lastMessage != null)
                    {

                        var userDto = await _userService.GetUserById(userFriend.FriendId);

                        if (userId == userFriend.FriendId)
                        {
                            userDto = await _userService.GetUserById(userFriend.UserId);

                        }
                        result.Add(new LastestMessage
                        {
                            UserId = userId,
                            Content = lastMessage.Content,
                            UserFriendInfo = userDto.Data,
                            Id = lastMessage.Id,
                            IsRead = lastMessage.IsRead,
                            SentOn = lastMessage.SentOn
                        });
                    }
                }
                return ApiResponse<LastestMessage[]>.Success(result.ToArray());
            }
            catch (Exception ex)
            {
                return ApiResponse<LastestMessage[]>.Fail(ex.Message);

            }
        }

        public async Task<ApiResponse<MessagesDto[]>> GetMessages(int fromUserId, int toUserId)
        {
            try
            {
                var entities = await _petContext.Messages
                .Where(x => (x.SenderId == fromUserId && x.ReceiverId == toUserId)
                    || (x.SenderId == toUserId && x.ReceiverId == fromUserId))
                .OrderBy(x => x.SentOn)
                .ToListAsync();

                var messagesDto = _mapper.Map<MessagesDto[]>(entities);
                return ApiResponse<MessagesDto[]>.Success(messagesDto);
            }
            catch (Exception ex)
            {
                return ApiResponse<MessagesDto[]>.Fail(ex.Message);

            }
        }
    }
}
