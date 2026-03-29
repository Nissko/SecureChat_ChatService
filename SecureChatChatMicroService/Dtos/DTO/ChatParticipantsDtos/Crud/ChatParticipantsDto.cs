using NodaTime;

namespace Dtos.DTO.ChatParticipantsDtos.Crud
{
    public record ChatParticipantsDto(
        Guid Id,
        Instant EnterTime,
        Instant? ExitTime,
        Guid ChatId,
        Guid UserId);
}