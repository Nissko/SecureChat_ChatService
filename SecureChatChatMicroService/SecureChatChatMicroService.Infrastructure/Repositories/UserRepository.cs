using Dtos.DTO.ChatGroupDtos.Crud;
using Dtos.DTO.GroupDtos.Crud;
using Dtos.DTO.UserDtos.Crud;
using Microsoft.EntityFrameworkCore;
using Requests.User;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IChatServiceDbContext _context;

        public UserRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<UserDto>> GetAll()
        {
            try
            {
                var users = await _context.User.ToListAsync();
                return GetUserDto(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> FromId(Guid id)
        {
            try
            {
                var user = await _context.User.FindAsync([id]);
                return user == null ? throw new Exception("User not found") : GetUserDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Guid> Create(CreateUserRequest request)
        {
            try
            {
                //TODO: подумать над тем, как мы будем создавать чаты
                var newUser = new UserEntity(request.UserProfileId, false);
                newUser.Groups.Add(new GroupEntity("Все чаты", newUser.Id));
                
                _context.User.Add(newUser);
                
                await SaveChanges();
                return newUser.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Update(UpdateUserRequest request)
        {
            try
            {
                var user = await _context.User.FindAsync([request.Id])
                           ?? throw new Exception("User not found");
                user.Update(request.UserProfileId, request.IsDeleted);

                _context.User.Update(user);
                await SaveChanges();

                return GetUserDto(user);
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
                var user = await _context.User.FindAsync([id])
                           ?? throw new Exception("User not found");
                if (user.IsDeleted) throw new Exception("User already is deleted");
                user.Update(null, false);

                _context.User.Update(user);
                await SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static UserDto GetUserDto(UserEntity e)
        {
            return new UserDto(
                e.Id,
                e.UserProfileId,
                e.IsDeleted,
                e.Groups.Select(gr => new GroupDto(
                    gr.Id,
                    gr.Name,
                    gr.UserId,
                    gr.ChatGroups
                        .Select(cgr => new ChatGroupDto(
                            cgr.Id,
                            cgr.ChatId,
                            cgr.GroupId))
                        .ToList()
                )).ToList()
            );
        }

        private static List<UserDto> GetUserDto(List<UserEntity> e)
        {
            return e.Select(u => new UserDto(
                u.Id,
                u.UserProfileId,
                u.IsDeleted,
                u.Groups.Select(gr => new GroupDto(
                    gr.Id,
                    gr.Name,
                    gr.UserId,
                    gr.ChatGroups
                        .Select(cgr => new ChatGroupDto(
                            cgr.Id,
                            cgr.ChatId,
                            cgr.GroupId))
                        .ToList())
                ).ToList())
            ).ToList();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}