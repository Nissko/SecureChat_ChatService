using NodaTime;

namespace SecureChatChatMicroService.Domain.Entities
{
    /// <summary>
    /// Пользователь
    /// TODO: настроить синхронизацию с UserChatMicroservice
    /// </summary>
    public class UserEntity
    {
        public UserEntity()
        {
            Groups = new HashSet<GroupEntity>();
            ChatParticipants = new HashSet<ChatParticipantsEntity>();
            Messages = new HashSet<MessageEntity>();
        }

        public UserEntity(Guid userId, Instant? deletedAt) : this()
        {
            UserId = userId;
            DeletedAt = deletedAt;
        }

        /// <summary>
        /// Идентификатор пользователя с сервиса User
        /// </summary>
        public Guid UserId { get; private set; }
        
        /// <summary>
        /// Удален ли пользователь
        /// </summary>
        public Instant? DeletedAt { get; private set; }

        public virtual ICollection<GroupEntity> Groups { get; private set; }
        public virtual ICollection<ChatParticipantsEntity> ChatParticipants { get; private set; }
        public virtual ICollection<MessageEntity> Messages { get; private set; }


        public void Update(Instant? deletedAt)
        {
            DeletedAt = deletedAt ?? DeletedAt;
        }
    }
}