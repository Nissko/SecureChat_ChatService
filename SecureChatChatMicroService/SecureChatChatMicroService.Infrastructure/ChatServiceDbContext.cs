using Microsoft.EntityFrameworkCore;
using SecureChatUserMicroService.Application.Common.Interfaces;

namespace SecureChatChatMicroService.Infrastructure;

public sealed class ChatServiceDbContext : DbContext, IChatServiceDbContext
    {
        private const string DefaultSchema = "ChatMicroService";

        public ChatServiceDbContext(DbContextOptions<ChatServiceDbContext> options)
            : base(options)
        {
        }
        
        //public DbSet<ChatEntity> Chat { get; set; }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DefaultSchema);

            #region user

            //modelBuilder.ApplyConfiguration(new ChatConfiguration());

            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatServiceDbContext).Assembly);
        }

        public ChatServiceDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql("Server=109.205.58.47;User Id=persProjectUser;Password=7FEpX_wl6g;Port=5432;Database=testDb;", npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
            optionsBuilder.UseNpgsql("Server=localhost;User Id=postgres;Password=0000;Port=5432;Database=postgres;", npgsqlOptions => { npgsqlOptions.UseNodaTime(); }).UseLazyLoadingProxies();
        }

        private static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .Options;
        }
    }