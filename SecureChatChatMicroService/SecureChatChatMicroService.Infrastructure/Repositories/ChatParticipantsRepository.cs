using Dtos.DTO.ChatParticipantsDtos.Crud;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.ChatParticipants;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class ChatParticipantsRepository : IChatParticipantsRepository
    {
        private readonly IChatServiceDbContext _context;

        public ChatParticipantsRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ChatParticipantsDto>> GetAll()
        {
            try
            {
                var chatParticipants = await _context.ChatParticipants.ToListAsync();
                return GetChatParticipantsDto(chatParticipants);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatParticipantsDto> FromId(Guid id)
        {
            try
            {
                var chatParticipant = await _context.ChatParticipants.FindAsync([id]) ??
                                      throw new Exception("Chat Participant not found");
                return GetChatParticipantsDto(chatParticipant);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> Create(CreateChatParticipantsRequest request)
        {
            try
            {
                var newChatParticipant = new ChatParticipantsEntity(SystemClock.Instance.GetCurrentInstant(), null,
                    request.ChatId, request.UserId);

                _context.ChatParticipants.Add(newChatParticipant);
                await SaveChanges();
                
                return newChatParticipant.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatParticipantsDto> Update(UpdateChatParticipantsRequest request)
        {
            try
            {
                var chatParticipant = await _context.ChatParticipants.FindAsync([request.Id]) ??
                                      throw new Exception("Chat Participant not found");
                chatParticipant.Update(request.ExitTime);
                
                _context.ChatParticipants.Update(chatParticipant);
                await SaveChanges();
                
                return GetChatParticipantsDto(chatParticipant);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var chatParticipant = await _context.ChatParticipants.FindAsync([id]) ??
                                      throw new Exception("Chat Participant not found");
                if (chatParticipant.UserId == chatParticipant.Chat.OwnerId && chatParticipant.Chat.OwnerId != null)
                {
                    throw new Exception("You cannot delete Owner Chat Participants");
                }
                
                _context.ChatParticipants.Remove(chatParticipant);
                await SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        private static ChatParticipantsDto GetChatParticipantsDto(ChatParticipantsEntity e)
        {
            return new ChatParticipantsDto(
                e.Id,
                e.EnterTime,
                e.ExitTime ?? null,
                e.ChatId,
                e.UserId
            );
        }

        private static List<ChatParticipantsDto> GetChatParticipantsDto(List<ChatParticipantsEntity> e)
        {
            return e.Select(u => new ChatParticipantsDto(
                u.Id,
                u.EnterTime,
                u.ExitTime ?? null,
                u.ChatId,
                u.UserId)
            ).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}