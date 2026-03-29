using Dtos.DTO.ChatGroupDtos.Crud;
using Dtos.DTO.ChatParticipantsDtos.Crud;
using Dtos.DTO.MessageDtos.Crud;
using NodaTime;

namespace Dtos.DTO.ChatDtos.Crud
{
    public record ChatDto(
        Guid Id,
        Instant LastMessageTime,
        int CountUnreadMessages,
        bool IsPint,
        bool IsMute,
        bool IsDeleted,
        Guid Type,
        Guid? OwnerId,
        IEnumerable<ChatGroupDto> ChatGroups,
        IEnumerable<ChatParticipantsDto> ChatParticipants,
        IEnumerable<MessageDto> Messages);
}