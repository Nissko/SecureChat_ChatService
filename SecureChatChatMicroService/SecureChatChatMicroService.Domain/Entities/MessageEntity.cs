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

        public MessageEntity(Guid? answerMessageId, Guid chatId, Guid chatParticipantsId, Instant sendTime,
            Instant? updateTime, Instant? deleteTime, string text, bool isDeleted = false)
        {
            AnswerMessageId = answerMessageId;
            ChatId = chatId;
            ChatParticipantsId = chatParticipantsId;
            SendTime = sendTime;
            UpdateTime = updateTime;
            DeleteTime = deleteTime;
            TextMessage = text;
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
        public Guid ChatParticipantsId { get; private set; }

        public virtual ChatParticipantsEntity ChatParticipant { get; private set; }

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
        public string TextMessage { get; private set; }

        /// <summary>
        /// Было ли удалено
        /// </summary>
        public bool IsDeleted { get; private set; }

        /*public void MarkAsEdited(Instant updateTime)
        {
            UpdateTime = updateTime;
        }

        public void SoftDelete(Instant deleteTime)
        {
            IsDeleted = true;
            DeleteTime = deleteTime;
        }*/

        /*public void SetAnswerMessage(Guid? answerMessageId)
        {
            AnswerMessageId = answerMessageId;
        }*/

        /*public void Update(string? content)
        {
            Content = content ?? Content;
        }*/
    }
}