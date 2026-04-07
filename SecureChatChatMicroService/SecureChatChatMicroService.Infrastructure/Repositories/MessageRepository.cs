using Dtos.DTO.MessageDtos.Crud;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.Message;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IChatServiceDbContext _context;

        public MessageRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<MessageDto>> GetAll(Guid chatId)
        {
            try
            {
                var chat = await _context.Chat.FindAsync([chatId]) ?? throw new("Chat not found");
                var messages = await _context.Message.Where(x => x.ChatId == chat.Id).ToListAsync();
                return GetMessageDto(messages);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<MessageDto> FromId(Guid id)
        {
            try
            {
                var message = await _context.Message.FindAsync([id]) ?? throw new("Message not found");
                return GetMessageDto(message);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<Guid> Create(CreateMessageRequest request)
        {
            try
            {
                var newMessage = new MessageEntity(request.ChatId, request.UserId,
                    SystemClock.Instance.GetCurrentInstant(), SystemClock.Instance.GetCurrentInstant(), request.Content,
                    request.TypeOfMessage, null, false, false, request.AnswerMessageId);
                
                _context.Message.Add(newMessage);
                await SaveChanges();
                
                return newMessage.Id;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<MessageDto> Update(UpdateMessageRequest request)
        {
            try
            {
                var message = await _context.Message.FindAsync([request.Id]) ??
                              throw new("Message not found");
                if (message.Content != request.Content)
                { 
                    message.Update(request.Content);
                    message.MarkAsEdited(SystemClock.Instance.GetCurrentInstant());
                }
                
                _context.Message.Update(message);
                await SaveChanges();
                
                return GetMessageDto(message);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var message = await _context.Message.FindAsync(id) ?? throw new("Message not found");
                message.SoftDelete(SystemClock.Instance.GetCurrentInstant());
                
                _context.Message.Update(message);
                await SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }
        
        private static MessageDto GetMessageDto(MessageEntity e)
        {
            return new(
                e.Id,
                e.AnswerMessageId,
                e.ChatId,
                e.UserId,
                e.TypeOfMessage,
                e.Content,
                e.SendTime,
                e.UpdateTime,
                e.DeleteTime,
                e.IsEdited,
                e.IsDeleted
            );
        }

        private static List<MessageDto> GetMessageDto(List<MessageEntity> e)
        {
            return e.Select(u => new MessageDto(
                u.Id,
                u.AnswerMessageId,
                u.ChatId,
                u.UserId,
                u.TypeOfMessage,
                u.Content,
                u.SendTime,
                u.UpdateTime,
                u.DeleteTime,
                u.IsEdited,
                u.IsDeleted)
            ).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}