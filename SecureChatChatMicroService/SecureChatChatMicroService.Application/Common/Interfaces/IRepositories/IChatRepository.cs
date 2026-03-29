using Dtos.DTO.ChatDtos.Crud;
using Requests.Chat;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IChatRepository
    {
        /// <summary>
        /// Получение всех
        /// </summary>
        Task<List<ChatDto>> GetAll();

        /// <summary>
        /// Получение
        /// </summary>
        Task<ChatDto> FromId(Guid id);

        /// <summary>
        /// Добавление
        /// </summary>
        Task<Guid> Create(CreateChatRequest request);

        /// <summary>
        /// Обновление
        /// </summary>
        Task<ChatDto> Update(UpdateChatRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}