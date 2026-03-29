using Dtos.DTO.GroupDtos.Crud;
using Requests.Group;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Получение всех
        /// </summary>
        Task<List<GroupDto>> GetAll();

        /// <summary>
        /// Получение
        /// </summary>
        Task<GroupDto> FromId(Guid id);

        /// <summary>
        /// Добавление
        /// </summary>
        Task<Guid> Create(CreateGroupRequest request);

        /// <summary>
        /// Обновление
        /// </summary>
        Task<GroupDto> Update(UpdateGroupRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}