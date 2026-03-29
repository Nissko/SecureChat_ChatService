using Dtos.DTO.ChatDtos.Crud;
using Dtos.DTO.ChatGroupDtos.Crud;
using Dtos.DTO.ChatParticipantsDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Requests.Chat;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;
using SecureChatChatMicroService.Domain.Enums;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IChatServiceDbContext _context;

        public ChatRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ChatDto>> GetAll()
        {
            try
            {
                var chats = await _context.Chat.ToListAsync();
                return GetChatDto(chats);
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatDto> FromId(Guid id)
        {
            try
            {
                var chat = await _context.Chat.FindAsync([id]) ?? throw new Exception("Chat not found");
                return GetChatDto(chat);
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> Create(CreateChatRequest request)
        {
            try
            {
                var chatTypeEnum = ChatTypeEnum.FromId(request.Type);
                var newChat = new ChatEntity(SystemClock.Instance.GetCurrentInstant(), 0, false, false, false,
                    chatTypeEnum.Id, request.OwnerId);
                newChat.ChatGroups.Add(new ChatGroupEntity(newChat.Id, request.ChatGroupId));
                _context.Chat.Add(newChat);
                await SaveChanges();
                
                return newChat.Id;
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public async Task<ChatDto> Update(UpdateChatRequest request)
        {
            try
            {
                var chat = await _context.Chat.FindAsync([request.Id]) ?? throw new Exception("Chat not found");
                chat.Update(request.LastMessageTime, request.CountUnreadMessages, request.IsPint, request.IsMute,
                    request.Type, null);
                var chatGroup = chat.ChatGroups.FirstOrDefault(x => x.ChatId == chat.Id) ??
                                throw new Exception("ChatGroup not found");
                chatGroup.Update(null, request.GroupId);
                
                _context.Chat.Update(chat);
                _context.ChatGroup.Update(chatGroup);
                await SaveChanges();
                
                return GetChatDto(chat);
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var chat = await _context.Chat.FindAsync([id]) ?? throw new Exception("Chat not found");
                if (chat.IsDeleted) throw new Exception("Chat already is deleted");

                chat.Update(null, null, null, null, null, true);
                _context.Chat.Update(chat);
                await SaveChanges();

                return true;
            }
            catch(Exception ex){
                throw new Exception(ex.Message);
            }
        }

        private static ChatDto GetChatDto(ChatEntity e)
        {
            return new ChatDto(
                e.Id,
                e.LastMessageTime,
                e.CountUnreadMessages,
                e.IsPint,
                e.IsMute,
                e.IsDeleted,
                e.Type,
                e.OwnerId ?? null,
                e.ChatGroups.Select(cgr => new ChatGroupDto(
                        cgr.Id,
                        cgr.ChatId,
                        cgr.ChatId))
                    .ToList(),
                e.ChatParticipants.Select(chp => new ChatParticipantsDto(
                        chp.Id,
                        chp.EnterTime,
                        chp.ExitTime ?? null,
                        chp.ChatId,
                        chp.UserId))
                    .ToList(),
                e.Messages.Select(chm => new MessageDto(
                    chm.Id,
                    chm.AnswerMessageId ?? null,
                    chm.ChatId,
                    chm.UserId,
                    chm.TypeOfMessage,
                    chm.Content,
                    chm.SendTime,
                    chm.UpdateTime ?? null,
                    chm.DeleteTime ?? null,
                    chm.IsEdited,
                    chm.IsDeleted)).ToList()
            );
        }

        private static List<ChatDto> GetChatDto(List<ChatEntity> e)
        {
            return e.Select(u => new ChatDto(
                u.Id,
                u.LastMessageTime,
                u.CountUnreadMessages,
                u.IsPint,
                u.IsMute,
                u.IsDeleted,
                u.Type,
                u.OwnerId ?? null,
                u.ChatGroups.Select(cgr => new ChatGroupDto(
                        cgr.Id,
                        cgr.ChatId,
                        cgr.ChatId))
                    .ToList(),
                u.ChatParticipants.Select(chp => new ChatParticipantsDto(
                        chp.Id,
                        chp.EnterTime,
                        chp.ExitTime ?? null,
                        chp.ChatId,
                        chp.UserId))
                    .ToList(),
                u.Messages.Select(chm => new MessageDto(
                    chm.Id,
                    chm.AnswerMessageId ?? null,
                    chm.ChatId,
                    chm.UserId,
                    chm.TypeOfMessage,
                    chm.Content,
                    chm.SendTime,
                    chm.UpdateTime ?? null,
                    chm.DeleteTime ?? null,
                    chm.IsEdited,
                    chm.IsDeleted)).ToList()
            )).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}