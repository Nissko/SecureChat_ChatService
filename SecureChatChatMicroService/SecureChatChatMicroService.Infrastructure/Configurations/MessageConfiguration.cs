using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("Messages");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SendTime)
                .HasColumnName("SendTime")
                .IsRequired()
                .HasComment("Дата отправки");

            builder.Property(x => x.UpdateTime)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasComment("Дата изменения");

            builder.Property(x => x.DeleteTime)
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasComment("Дата удаления");

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("Удалено ли");

            builder.Property(x => x.TextMessage)
                .IsRequired()
                .HasComment("Текст сообщения");

            builder.HasOne(x => x.ChatParticipant)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatParticipantsId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Chat)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChatId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(m => m.AnswerMessage)
                .WithMany(m => m.RepliesMessage)
                .HasForeignKey(m => m.AnswerMessageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}