using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Enums;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class ChatTypeConfiguration : IEntityTypeConfiguration<ChatTypeEnum>
    {
        public void Configure(EntityTypeBuilder<ChatTypeEnum> builder)
        {
            builder.ToTable("ChatTypes");

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .HasComment("Тип чата");

            builder.HasIndex(x => x.Id);
        }
    }
}