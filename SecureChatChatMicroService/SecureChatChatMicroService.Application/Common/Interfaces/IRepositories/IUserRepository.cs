namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Добавление пользователя
        /// </summary>
        Task<bool> AddUser(Guid userId);
        
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        Task<bool> RemoveUser(Guid userId);
    }
}