using Dtos.DTO;
using Dtos.DTO.ChatDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.Chat;
using Requests.Message;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;
using SecureChatChatMicroService.Domain.Enums;


namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatServiceDbContext _context;
        private const string DefaultGroupName = "Все чаты";

        public ChatRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<ChatDto> CreateChat(CreateChatRequest createChatRequest)
        {
            try
            {
                var newChat = new ChatEntity(null, ChatTypeEnum.ChatType.Id, false);
                foreach (var newParticipantId in createChatRequest.ParticipantIds)
                {
                    newChat.ChatParticipants.Add(new ChatParticipantsEntity(SystemClock.Instance.GetCurrentInstant(),
                        null, newChat.Id, newParticipantId));
                    var user = await _context.User.FirstOrDefaultAsync(x => x.UserId == newParticipantId) ??
                               throw new NullReferenceException("User not found");
                    var defaultUserGroup = user.Groups.FirstOrDefault(x => x.Name == DefaultGroupName) ??
                                           throw new NullReferenceException("Default user group not found");
                    newChat.ChatGroups.Add(new ChatGroupEntity(newChat.Id, defaultUserGroup.Id));
                }

                _context.Chat.Add(newChat);
                await SaveChanges();

                return GetChatDto(newChat);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationDto<ChatDto>> GetUserChats(GetUserChatsRequest getUserChatsRequest)
        {
            try
            {
                var chatParticipants = await _context.ChatParticipants
                    .Where(x => x.UserId == getUserChatsRequest.UserId)
                    .OrderByDescending(x => x.EnterTime)
                    .Skip(getUserChatsRequest.Offset)
                    .Take(getUserChatsRequest.Limit)
                    .Include(x => x.Chat)
                    .ThenInclude(c => c.Messages)
                    .Include(x => x.Chat)
                    .ThenInclude(c => c.ChatParticipants)
                    .Include(x => x.Chat)
                    .ThenInclude(c => c.ChatGroups)
                    .ToListAsync();
                var chats = chatParticipants.Select(x => x.Chat).ToList();

                return new PaginationDto<ChatDto>(GetChatDtos(chats), chats.Count);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PaginationDto<MessageDto>> GetMessages(GetMessagesRequest getMessagesRequest)
        {
            try
            {
                var totalCount = await _context.Message
                    .Where(x => x.ChatId == getMessagesRequest.ChatId)
                    .CountAsync();
                var messages = await _context.Message
                    .Where(x => x.ChatId == getMessagesRequest.ChatId)
                    .OrderByDescending(x => x.SendTime)
                    .Skip(getMessagesRequest.Offset)
                    .Take(getMessagesRequest.Limit)
                    .ToListAsync();
                return new PaginationDto<MessageDto>(GetMessageDtos(messages), totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatDto> GetChatInfo(GetChatInfoRequest getChatInfoRequest)
        {
            try
            {
                var chat = await _context.Chat.FirstOrDefaultAsync(x => x.Id == getChatInfoRequest.ChatId) ??
                           throw new NullReferenceException("Chat not found");
                return GetChatDto(chat);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SendMessage(SendMessageRequest sendMessageRequest)
        {
            try
            {
                var chatParticipant = _context.ChatParticipants
                                          .FirstOrDefault(x => x.UserId == sendMessageRequest.ChatParticipantId) ??
                                      throw new NullReferenceException("Chat participant not found");
                var answerMessageId = sendMessageRequest.AnswerMessageId == Guid.Empty
                    ? null
                    : sendMessageRequest.AnswerMessageId;
                var newMessage = new MessageEntity(answerMessageId, sendMessageRequest.ChatId,
                    chatParticipant.Id, SystemClock.Instance.GetCurrentInstant(), null, null,
                    sendMessageRequest.Text);
                
                _context.Message.Add(newMessage);
                await SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static ChatDto GetChatDto(ChatEntity e)
        {
            var lastMessage = e.Messages.LastOrDefault();
            return new(
                e.Id,
                e.ChatParticipants.Select(x => x.UserId).ToList(),
                lastMessage?.TextMessage,
                lastMessage?.SendTime
            );
        }

        private static List<ChatDto> GetChatDtos(List<ChatEntity> en)
        {
            return en.Select(e =>
            {
                var lastMessage = e.Messages.LastOrDefault();
                return new ChatDto(
                    e.Id,
                    e.ChatParticipants.Select(x => x.UserId).ToList(),
                    lastMessage?.TextMessage,
                    lastMessage?.SendTime
                );
            }).ToList();
        }

        private static MessageDto GetMessageDto(MessageEntity e)
        {
            return new(
                e.Id,
                e.ChatId,
                e.ChatParticipantsId,
                e.TextMessage,
                e.SendTime
            );
        }

        private static List<MessageDto> GetMessageDtos(List<MessageEntity> en)
        {
            return en.Select(e => new MessageDto(
                e.Id,
                e.ChatId,
                e.ChatParticipantsId,
                e.TextMessage,
                e.SendTime
            )).ToList();
        }
    }
}