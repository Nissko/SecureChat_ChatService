using Dtos.DTO.ChatGroupDtos.Crud;
using Requests.ChatGroup;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IChatGroupRepository
    {
        /// <summary>
        /// Получение всех
        /// </summary>
        Task<List<ChatGroupDto>> GetAll();

        /// <summary>
        /// Получение
        /// </summary>
        Task<ChatGroupDto> FromId(Guid id);

        /// <summary>
        /// Добавление
        /// </summary>
        Task<Guid> Create(CreateChatGroupRequest request);

        /// <summary>
        /// Обновление
        /// </summary>
        Task<ChatGroupDto> Update(UpdateChatGroupRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}