using NodaTime;
using SecureChatChatMicroService.Domain.Common;

namespace SecureChatChatMicroService.Domain.Entities
{
    public class MessageEntity : Entity
    {
        public MessageEntity()
        {
            RepliesMessage = new HashSet<MessageEntity>();
        }

        public MessageEntity(Guid chatId, Guid userId, Instant sendTime, Instant updateTime, string content,
            Guid typeOfMessage, Instant? deleteTime, bool isEdited, bool isDeleted, Guid? answerMessageId) : this()
        {
            AnswerMessageId = answerMessageId;
            ChatId = chatId;
            UserId = userId;
            Content = content;
            TypeOfMessage = typeOfMessage;
            SendTime = sendTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
            IsEdited = isEdited;
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// Ид ответа на сообщение
        /// </summary>
        public Guid? AnswerMessageId { get; private set; }

        public virtual MessageEntity? AnswerMessage { get; private set; }
        public virtual ICollection<MessageEntity> RepliesMessage { get; private set; }

        /// <summary>
        /// Ид чата
        /// </summary>
        public Guid ChatId { get; private set; }

        public virtual ChatEntity Chat { get; private set; }

        /// <summary>
        /// Ид пользователя
        /// </summary>
        public Guid UserId { get; private set; }

        public virtual UserEntity User { get; private set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public Instant SendTime { get; private set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public Instant? UpdateTime { get; private set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public Instant? DeleteTime { get; private set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Тип (сообщение, картинка)
        /// </summary>
        public Guid TypeOfMessage { get; private set; }

        /// <summary>
        /// Было ли изменено
        /// </summary>
        public bool IsEdited { get; private set; }

        /// <summary>
        /// Было ли удалено
        /// </summary>
        public bool IsDeleted { get; private set; }

        public void MarkAsEdited(Instant updateTime)
        {
            IsEdited = true;
            UpdateTime = updateTime;
        }

        public void SoftDelete(Instant deleteTime)
        {
            IsDeleted = true;
            DeleteTime = deleteTime;
        }

        public void SetAnswerMessage(Guid? answerMessageId)
        {
            AnswerMessageId = answerMessageId;
        }

        public void Update(string? content)
        {
            Content = content ?? Content;
        }
    }
}