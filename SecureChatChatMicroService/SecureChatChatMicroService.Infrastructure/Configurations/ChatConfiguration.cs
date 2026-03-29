using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<ChatEntity>
    {
        public void Configure(EntityTypeBuilder<ChatEntity> builder)
        {
            builder.ToTable("Chats");
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.LastMessageTime)
                .HasColumnName("LastMessageTime")
                .IsRequired()
                .HasComment("Дата последнего сообщения");
            
            builder.Property(x => x.CountUnreadMessages)
                .HasColumnName("CountUnreadMessages")
                .HasDefaultValue(0)
                .HasComment("Кол-во непрочитанных сообщений");
            
            builder.Property(x => x.IsPint)
                .HasColumnName("IsPint")
                .HasDefaultValue(false)
                .HasComment("Закреплен ли чат");
            
            builder.Property(x => x.IsMute)
                .HasColumnName("IsMute")
                .HasDefaultValue(false)
                .HasComment("Показывать ли уведомления");
            
            builder.Property(x => x.IsDeleted)
                .HasColumnName("IsDeleted")
                .HasDefaultValue(false)
                .HasComment("Удален ли");
            
            builder.Property(x => x.Type)
                .HasColumnName("Type")
                .IsRequired()
                .HasComment("Тип (чат, канал, группа)");
            
            builder.Property(x => x.OwnerId)
                .HasColumnName("OwnerId")
                .IsRequired(false)
                .HasComment("Создатель чата");
        }
    }
}