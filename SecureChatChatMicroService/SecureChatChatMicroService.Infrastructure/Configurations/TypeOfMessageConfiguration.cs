using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Enums;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class TypeOfMessageConfiguration : IEntityTypeConfiguration<TypeOfMessageEnum>
    {
        public void Configure(EntityTypeBuilder<TypeOfMessageEnum> builder)
        {
            builder.ToTable("TypeOfMessages");

            builder.Property(o => o.Id)
                .ValueGeneratedNever();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .HasComment("Тип сообщения");

            builder.HasIndex(x => x.Id);
        }
    }
}