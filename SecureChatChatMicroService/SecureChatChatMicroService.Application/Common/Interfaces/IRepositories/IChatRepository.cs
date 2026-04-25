using Dtos.DTO;
using Dtos.DTO.ChatDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;
using Requests.Chat;
using Requests.Message;

namespace SecureChatChatMicroService.Application.Common.Interfaces.IRepositories
{
    public interface IChatRepository
    {
        /// <summary>
        /// Создание нового чата
        /// </summary>
        Task<ChatDto> CreateChat(CreateChatRequest createChatRequest);

        /// <summary>
        /// Получение списка чатов пользователя
        /// </summary>
        Task<PaginationDto<ChatDto>> GetUserChats(GetUserChatsRequest getUserChatsRequest);

        /// <summary>
        /// Получение истории сообщений чата
        /// </summary>
        Task<PaginationDto<MessageDto>> GetMessages(GetMessagesRequest getMessagesRequest);

        /// <summary>
        /// Получение информации о чате
        /// </summary>
        Task<ChatDto> GetChatInfo(GetChatInfoRequest getChatInfoRequest);

        Task SendMessage(SendMessageRequest sendMessageRequest);
    }
}