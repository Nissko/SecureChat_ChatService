using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    public class GroupEntity : Entity
    {
        public GroupEntity()
        {
            ChatGroups = new HashSet<ChatGroupEntity>();
        }
    
        public GroupEntity(string name, Guid userId) : this()
        {
            Name = name;
            UserId = userId;
        }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; private set; }
    
        /// <summary>
        /// Пользователь
        /// </summary>
        public Guid UserId { get; private set; }
        public virtual UserEntity User { get; private set; }
    
        public virtual ICollection<ChatGroupEntity> ChatGroups { get; private set; }

        public void Update(string? name, Guid? userId)
        {
            Name = name ?? Name;
            UserId = userId ?? UserId;
        }
    }
}