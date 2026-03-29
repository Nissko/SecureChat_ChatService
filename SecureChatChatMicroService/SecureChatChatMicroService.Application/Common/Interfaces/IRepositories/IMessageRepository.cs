using Dtos.DTO.MessageDtos.Crud;
using Requests.Message;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IMessageRepository
    {
        /// <summary>
        /// Получение всех
        /// </summary>
        Task<List<MessageDto>> GetAll(Guid chatId);

        /// <summary>
        /// Получение
        /// </summary>
        Task<MessageDto> FromId(Guid id);

        /// <summary>
        /// Добавление
        /// </summary>
        Task<Guid> Create(CreateMessageRequest request);

        /// <summary>
        /// Обновление
        /// </summary>
        Task<MessageDto> Update(UpdateMessageRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}