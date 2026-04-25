using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class ChatParticipantsConfiguration : IEntityTypeConfiguration<ChatParticipantsEntity>
    {
        public void Configure(EntityTypeBuilder<ChatParticipantsEntity> builder)
        {
            builder.ToTable("ChatParticipants");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EnterTime)
                .IsRequired()
                .HasComment("Дата входа");
            
            builder.Property(x => x.ExitTime)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasComment("Дата выхода");

            builder.Property(x => x.IsPint)
                .HasDefaultValue(false)
                .HasComment("Закреплен ли чат у пользователя");
            
            builder.Property(x => x.IsMuted)
                .HasDefaultValue(false)
                .HasComment("Есть ли уведомления от чата у пользователя");
            
            builder.HasOne(x => x.User)
                .WithMany(x => x.ChatParticipants)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(x => x.Chat)
                .WithMany(x => x.ChatParticipants)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}