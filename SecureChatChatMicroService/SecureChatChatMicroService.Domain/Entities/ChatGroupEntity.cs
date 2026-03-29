using System.Text.RegularExpressions;
using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    public class ChatGroupEntity : Entity
    {
        public ChatGroupEntity(Guid chatId, Guid groupId)
        {
            ChatId = chatId;
            GroupId = groupId;
        }

        /// <summary>
        /// Ид чата
        /// </summary>
        public Guid ChatId { get; private set; }
        public virtual ChatEntity Chat { get; private set; }
    
        /// <summary>
        /// Ид группы
        /// </summary>
        public Guid GroupId { get; private set; }
        public virtual Group Group { get; private set; }
        
        public void SetDefaultGroup(Guid defaultGroupId)
        {
            GroupId = defaultGroupId;
        }

        public void Update(Guid? chatId, Guid? groupId)
        {
            ChatId = chatId ?? ChatId;
            GroupId = groupId ?? GroupId;
        }
    }
}