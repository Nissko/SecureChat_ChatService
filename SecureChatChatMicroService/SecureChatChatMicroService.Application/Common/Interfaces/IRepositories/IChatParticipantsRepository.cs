using Dtos.DTO.ChatParticipantsDtos.Crud;
using Requests.ChatParticipants;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IChatParticipantsRepository
    {
        /// <summary>
        /// Получение всех
        /// </summary>
        Task<List<ChatParticipantsDto>> GetAll();

        /// <summary>
        /// Получение
        /// </summary>
        Task<ChatParticipantsDto> FromId(Guid id);

        /// <summary>
        /// Добавление
        /// </summary>
        Task<Guid> Create(CreateChatParticipantsRequest request);

        /// <summary>
        /// Обновление
        /// </summary>
        Task<ChatParticipantsDto> Update(UpdateChatParticipantsRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}