using NodaTime;
using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    public class ChatEntity : Entity
    {
        public ChatEntity()
        {
            ChatGroups = new HashSet<ChatGroupEntity>();
            ChatParticipants = new HashSet<ChatParticipantsEntity>();
            Messages = new HashSet<MessageEntity>();
        }

        public ChatEntity(Instant lastMessageTime, int countUnreadMessages, bool isPint, bool isMute, bool isDeleted,
            Guid type, Guid? ownerId) : this()
        {
            LastMessageTime = lastMessageTime;
            CountUnreadMessages = countUnreadMessages;
            IsPint = isPint;
            IsMute = isMute;
            IsDeleted = isDeleted;
            Type = type;
            OwnerId = ownerId;
        }

        /// <summary>
        /// Дата последнего сообщения
        /// </summary>
        public Instant LastMessageTime { get; private set; }

        /// <summary>
        /// Кол-во непрочитанных сообщений
        /// TODO: для каналов, групп сделать отдельно. Это только для чатов
        /// </summary>
        public int CountUnreadMessages { get; private set; }

        /// <summary>
        /// Закреплен ли чат
        /// </summary>
        public bool IsPint { get; private set; }

        /// <summary>
        /// Показывать ли уведомления
        /// </summary>
        public bool IsMute { get; private set; }

        /// <summary>
        /// Удален ли
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Тип (Чат, канал, группа)
        /// </summary>
        public Guid Type { get; private set; }

        /// <summary>
        /// Создатель чата
        /// TODO: Как быть если это 1vs1 чат, заполнять или пустое? Потестить крч
        /// </summary>
        public Guid? OwnerId { get; private set; }

        public virtual ICollection<ChatGroupEntity> ChatGroups { get; private set; }
        public virtual ICollection<ChatParticipantsEntity> ChatParticipants { get; private set; }
        public virtual ICollection<MessageEntity> Messages { get; private set; }

        public void Update(Instant? lastMessageTime, int? countUnreadMessages, bool? isPint, bool? isMute, Guid? type,
            bool? isDeleted)
        {
            LastMessageTime = lastMessageTime ?? LastMessageTime;
            CountUnreadMessages = countUnreadMessages ?? CountUnreadMessages;
            IsPint = isPint ?? IsPint;
            IsMute = isMute ?? IsMute;
            IsDeleted = isDeleted ?? IsDeleted;
            Type = type ?? Type;
        }
    }
}