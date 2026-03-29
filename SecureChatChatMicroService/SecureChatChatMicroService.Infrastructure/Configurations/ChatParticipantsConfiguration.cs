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
                .HasColumnName("EnterTime")
                .IsRequired()
                .HasComment("Дата входа");
            
            builder.Property(x => x.ExitTime)
                .HasColumnName("ExitTime")
                .IsRequired(false)
                .HasComment("Дата выхода");
            
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