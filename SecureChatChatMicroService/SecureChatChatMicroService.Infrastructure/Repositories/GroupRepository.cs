using Dtos.DTO.ChatGroupDtos.Crud;
using Dtos.DTO.GroupDtos.Crud;
using Microsoft.EntityFrameworkCore;
using Requests.Group;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IChatServiceDbContext _context;
        private const string DefaultGroupName = "Все чаты";

        public GroupRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<GroupDto>> GetAll()
        {
            try
            {
                var groups = await _context.Group.ToListAsync();
                return GetGroupDto(groups);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<GroupDto> FromId(Guid id)
        {
            try
            {
                var group = await _context.Group.FindAsync([id]);
                return group == null ? throw new("Group not found") : GetGroupDto(group);
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<Guid> Create(CreateGroupRequest request)
        {
            try
            {
                var user = await _context.User.FindAsync([request.UserId]) ?? throw new("User not found");
                var newGroup = new GroupEntity(request.Name, user.Id);
                
                _context.Group.Add(newGroup);
                await SaveChanges();
                
                return newGroup.Id;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        public async Task<GroupDto> Update(UpdateGroupRequest request)
        {
            try
            {
                var group = await _context.Group.FindAsync([request.Id]) ?? throw new("Group not found");
                if (request.Name == DefaultGroupName) throw new("Name cannot match the default one");
                group.Update(request.Name, request.UserId);
                
                _context.Group.Attach(group);
                await SaveChanges();
                
                return GetGroupDto(group);
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
                var group = await _context.Group.FindAsync([id]) ?? throw new("Group not found");
                var defaultGroup = group.User.Groups.FirstOrDefault(x => x.Name == DefaultGroupName) ??
                                   throw new("Default group not found");
                if (group.Id == defaultGroup.Id || group.Name == DefaultGroupName)
                {
                    throw new("Default group cannot be deleted");
                }
                
                // Переносим все чаты из кастомной группы в группу по умолчанию
                var chatGroups = group.ChatGroups.ToList();
                foreach (var chatGroup in chatGroups)
                {
                    chatGroup.SetDefaultGroup(defaultGroup.Id);
                }
                
                _context.ChatGroup.UpdateRange(chatGroups);
                _context.Group.Remove(group);
                await SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new(ex.Message);
            }
        }

        private static GroupDto GetGroupDto(GroupEntity e)
        {
            return new(
                e.Id,
                e.Name,
                e.UserId,
                e.ChatGroups
                    .Select(cgr => new ChatGroupDto(
                        cgr.Id,
                        cgr.ChatId,
                        cgr.GroupId))
                    .ToList()
            );
        }

        private static List<GroupDto> GetGroupDto(List<GroupEntity> e)
        {
            return e.Select(u => new GroupDto(
                u.Id,
                u.Name,
                u.UserId,
                u.ChatGroups
                    .Select(cgr => new ChatGroupDto(
                        cgr.Id,
                        cgr.ChatId,
                        cgr.GroupId))
                    .ToList())
            ).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}