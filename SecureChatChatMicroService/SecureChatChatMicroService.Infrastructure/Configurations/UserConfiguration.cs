using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecureChatChatMicroService.Domain.Entities;

namespace SecureChatChatMicroService.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
        
            builder.Property(x => x.UserProfileId)
                .HasColumnName("UserProfileId")
                .IsRequired()
                .HasComment("Ид профиля пользователя");
        
            builder.Property(x => x.IsDeleted)
                .HasColumnName("IsDeleted")
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Удален ли");
        }
    }
}