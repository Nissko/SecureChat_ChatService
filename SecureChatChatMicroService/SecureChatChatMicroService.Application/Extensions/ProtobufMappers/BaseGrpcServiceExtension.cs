using ChatService.Proto;
using Dtos.DTO.ChatDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;

namespace SecureChatChatMicroService.Application.Extensions.ProtobufMappers
{
    public static class BaseGrpcServiceExtension
    {
        #region Chat

        public static ChatResponse ToProtoChatInfo(this ChatDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                ParticipantIds = { dto.ParticipantIds.Select(id => id.ToString()).ToList() },
                LastMessage = dto.LastMessage ?? "",
                LastMessageAt = dto.LastMessageAt?.ToTimestamp() ?? null
            };
        }

        public static List<ChatResponse> ToProtoChatInfoList(
            this List<ChatDto> dtos)
        {
            return dtos.Select(ToProtoChatInfo).ToList();
        }

        public static Message ToProtoChatMessagesInfo(this MessageDto dto)
        {
            return new()
            {
                Id = dto.Id.ToString(),
                ChatId =  dto.ChatId.ToString(),
                ChatParticipantId = dto.ChatParticipantId.ToString(),
                Text =  dto.TextMessage,
                Timestamp = dto.Timestamp.ToTimestamp()
            };
        }
        
        public static List<Message> ToProtoChatMessagesInfoList(
            this List<MessageDto> dtos)
        {
            return dtos.Select(ToProtoChatMessagesInfo).ToList();
        }
        
        #endregion
    }
}