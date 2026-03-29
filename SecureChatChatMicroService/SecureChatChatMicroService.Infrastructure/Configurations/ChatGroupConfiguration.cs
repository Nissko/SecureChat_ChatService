using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class ChatGroupConfiguration : IEntityTypeConfiguration<ChatGroupEntity>
    {
        public void Configure(EntityTypeBuilder<ChatGroupEntity> builder)
        {
            builder.ToTable("ChatGroup");
            builder.HasKey(x => x.Id);
        
            builder.Property(x => x.ChatId)
                .HasColumnName("ChatId")
                .IsRequired()
                .HasComment("Ид чата");
            
            builder.Property(x => x.GroupId)
                .HasColumnName("GroupId")
                .IsRequired()
                .HasComment("Ид группы");
        }
    }
}