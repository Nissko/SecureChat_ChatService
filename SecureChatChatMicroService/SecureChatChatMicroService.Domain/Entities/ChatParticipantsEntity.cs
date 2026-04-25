using NodaTime;
using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    public class ChatParticipantsEntity : Entity
    {
        public ChatParticipantsEntity()
        {
            Messages = new HashSet<MessageEntity>();
        }

        public ChatParticipantsEntity(Instant enterTime, Instant? exitTime, Guid chatId, Guid userId,
            bool isPint = false, bool isMuted = false) : this()
        {
            EnterTime = enterTime;
            ExitTime = exitTime;
            ChatId = chatId;
            UserId = userId;
            IsPint = isPint;
            IsMuted = isMuted;
        }

        public Instant EnterTime { get; private set; }
        public Instant? ExitTime { get; private set; }
        public bool IsPint { get; private set; }
        public bool IsMuted { get; private set; }

        public Guid ChatId { get; private set; }
        public virtual ChatEntity Chat { get; private set; }

        public Guid UserId { get; private set; }
        public virtual UserEntity User { get; private set; }

        public virtual ICollection<MessageEntity> Messages { get; private set; }

        public void Update(Instant? exitTime, bool? isPint, bool? isMuted)
        {
            ExitTime = exitTime ?? ExitTime;
            IsPint = isPint ?? IsPint;
            IsMuted = isMuted ?? IsMuted;
        }
    }
}