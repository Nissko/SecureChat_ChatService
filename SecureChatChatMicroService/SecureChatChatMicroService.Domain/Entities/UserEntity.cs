using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    /// <summary>
    /// Пользователь
    /// TODO: настроить синхронизацию с UserChatMicroservice
    /// </summary>
    public class UserEntity : Entity
    {
        public UserEntity()
        {
            Groups = new HashSet<GroupEntity>();
            ChatParticipants = new HashSet<ChatParticipantsEntity>();
            Messages = new HashSet<MessageEntity>();
        }

        public UserEntity(Guid userProfileId, bool isDeleted) : this()
        {
            UserProfileId = userProfileId;
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// Ид профиля пользователя
        /// </summary>
        public Guid UserProfileId { get; private set; }

        /// <summary>
        /// Удален ли
        /// </summary>
        public bool IsDeleted { get; private set; }

        public virtual ICollection<GroupEntity> Groups { get; private set; }
        public virtual ICollection<ChatParticipantsEntity> ChatParticipants { get; private set; }
        public virtual ICollection<MessageEntity> Messages { get; private set; }

        public void Update(bool? isDeleted)
        {
            IsDeleted = isDeleted ?? IsDeleted;
        }
    }
}