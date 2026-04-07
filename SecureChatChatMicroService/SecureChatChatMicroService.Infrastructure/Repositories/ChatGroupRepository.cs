using Dtos.DTO.ChatGroupDtos.Crud;
using Microsoft.EntityFrameworkCore;
using Requests.ChatGroup;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class ChatGroupRepository : IChatGroupRepository
    {
        private readonly IChatServiceDbContext _context;

        public ChatGroupRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<ChatGroupDto>> GetAll()
        {
            var chatGroups = await _context.ChatGroup.ToListAsync();
            return ChatGroupDto(chatGroups);
        }

        public async Task<ChatGroupDto> FromId(Guid id)
        {
            var chatGroup = await _context.ChatGroup.FindAsync([id]) ?? throw new("Chat group not found");
            return ChatGroupDto(chatGroup);
        }

        public async Task<Guid> Create(CreateChatGroupRequest request)
        {
            var newChatGroup = new ChatGroupEntity(request.ChatId, request.GroupId);
            
            _context.ChatGroup.Add(newChatGroup);
            await SaveChanges();
            
            return newChatGroup.Id;
        }

        public async Task<ChatGroupDto> Update(UpdateChatGroupRequest request)
        {
            var chatGroup = await _context.ChatGroup.FindAsync([request.Id]) ??
                            throw new("Chat group not found");
            chatGroup.Update(request.ChatId, request.GroupId);
            
            _context.ChatGroup.Update(chatGroup);
            await SaveChanges();
            
            return ChatGroupDto(chatGroup);
        }
        
        private static ChatGroupDto ChatGroupDto(ChatGroupEntity e)
        {
            return new(
                e.Id,
                e.ChatId,
                e.GroupId
            );
        }

        private static List<ChatGroupDto> ChatGroupDto(List<ChatGroupEntity> e)
        {
            return e.Select(u => new ChatGroupDto(
                u.Id,
                u.ChatId,
                u.GroupId)
            ).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}