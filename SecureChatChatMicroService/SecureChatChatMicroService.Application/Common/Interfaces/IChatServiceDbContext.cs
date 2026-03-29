using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Application.Common.Interfaces
{
    public interface IChatServiceDbContext
    {
        DatabaseFacade Database { get; }
    
        public DbSet<UserEntity> User { get; set; }
        public DbSet<GroupEntity> Group { get; set; }
        public DbSet<ChatGroupEntity> ChatGroup { get; set; }
        public DbSet<ChatEntity> Chat { get; set; }
        public DbSet<MessageEntity> Message { get; set; }
        public DbSet<ChatParticipantsEntity> ChatParticipants { get; set; }

        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}