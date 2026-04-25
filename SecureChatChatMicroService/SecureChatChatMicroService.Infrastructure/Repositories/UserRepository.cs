using Microsoft.EntityFrameworkCore;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Domain.Entities;
using SystemClock = NodaTime.SystemClock;

namespace SecureChatChatMicroService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IChatServiceDbContext _context;
        private const string DefaultGroupName = "Все чаты";

        public UserRepository(IChatServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<bool> AddUser(Guid userId)
        {
            try
            {
                var user = new UserEntity(userId, null);
                user.Groups.Add(new GroupEntity(DefaultGroupName, user.UserId));
                
                _context.User.Add(user);
                await SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveUser(Guid userId)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(x => x.UserId == userId) ??
                           throw new NullReferenceException("User not found");
                user.Update(SystemClock.Instance.GetCurrentInstant());
                
                _context.User.Update(user);
                await SaveChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task SaveChanges() =>
            await _context.SaveChangesAsync(CancellationToken.None);
    }
}