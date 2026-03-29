using Dtos.DTO.UserDtos.Crud;
using Requests.User;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        Task<List<UserDto>> GetAll();

        /// <summary>
        /// Получение пользователя
        /// </summary>
        Task<UserDto> FromId(Guid id);

        /// <summary>
        /// Добавление нового пользователя
        /// </summary>
        Task<Guid> Create(CreateUserRequest request);

        /// <summary>
        /// Обновление информации
        /// </summary>
        Task<UserDto> Update(UpdateUserRequest request);

        /// <summary>
        /// Удаление
        /// </summary>
        Task<bool> Delete(Guid id);
    }
}