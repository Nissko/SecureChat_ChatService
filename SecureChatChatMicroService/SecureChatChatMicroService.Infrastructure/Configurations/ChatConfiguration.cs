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
                .IsRequired(false)
                .HasDefaultValue(null)
                .HasComment("Дата последнего сообщения");
            
            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("Удален ли");
            
            builder.Property(x => x.Type)
                .HasColumnName("Type")
                .IsRequired()
                .HasComment("Тип (чат, канал, группа)");
        }
    }
}