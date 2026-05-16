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

        public ChatEntity(Instant? lastMessageTime, Guid type, bool isDeleted) : this()
        {
            LastMessageTime = lastMessageTime;
            Type = type;
            IsDeleted = isDeleted;
        }

        public Instant? LastMessageTime { get; private set; }
        public Guid Type { get; private set; }
        public bool IsDeleted { get; private set; }
        
        public virtual ICollection<ChatGroupEntity> ChatGroups { get; private set; }
        public virtual ICollection<ChatParticipantsEntity> ChatParticipants { get; private set; }
        public virtual ICollection<MessageEntity> Messages { get; private set; }

        public void Update(Instant? lastMessageTime, Guid? type, bool? isDeleted)
        {
            LastMessageTime = lastMessageTime ?? LastMessageTime;
            IsDeleted = isDeleted ?? IsDeleted;
            Type = type ?? Type;
        }

        public void UpdateLastMessageTime(Instant? lastMessageTime)
        {
            LastMessageTime = lastMessageTime ?? LastMessageTime;
        }
    }
}