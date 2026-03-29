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
                .HasColumnName("UpdateTime")
                .IsRequired(false)
                .HasComment("Дата изменения");

            builder.Property(x => x.DeleteTime)
                .HasColumnName("DeleteTime")
                .IsRequired(false)
                .HasComment("Дата удаления");

            builder.Property(x => x.IsEdited)
                .HasColumnName("IsEdited")
                .HasDefaultValue(false)
                .HasComment("Изменено ли");

            builder.Property(x => x.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false)
                .HasComment("Удалено ли");
            
            builder.Property(x => x.Content)
                .HasColumnName("Content")
                .IsRequired()
                .HasComment("Текст сообщения");
            
            builder.Property(x => x.TypeOfMessage)
                .HasColumnName("TypeOfMessage")
                .IsRequired()
                .HasComment("Тип сообщения");

            builder.HasOne(x => x.User)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId)
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