using NodaTime;

namespace Dtos.DTO.MessageDtos.Crud
{
    public record MessageDto(
        Guid Id,
        Guid ChatId,
        Guid ChatParticipantId,
        string TextMessage,
        Instant Timestamp);
}