using Microsoft.EntityFrameworkCore;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Domain.Entities;
using SecureChatChatMicroService.Infrastructure.Configurations;

namespace SecureChatChatMicroService.Infrastructure
{
    public sealed class ChatServiceDbContext : DbContext, IChatServiceDbContext
    {
        private const string DefaultSchema = "ChatMicroService";

        public ChatServiceDbContext(DbContextOptions<ChatServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> User { get; set; }
        public DbSet<GroupEntity> Group { get; set; }
        public DbSet<ChatGroupEntity> ChatGroup { get; set; }
        public DbSet<ChatEntity> Chat { get; set; }
        public DbSet<MessageEntity> Message { get; set; }
        public DbSet<ChatParticipantsEntity> ChatParticipants { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            #region chat

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new ChatGroupConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatParticipantsConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatServiceDbContext).Assembly);
        }

        public ChatServiceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=radiomgn.ru;User Id=nikita;Password=qwertyaib12345678;Port=4444;Database=chat;",
                npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }
}