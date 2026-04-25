using NodaTime;

namespace Dtos.DTO.ChatDtos.Crud
{
    public record ChatDto(
        Guid Id,
        List<Guid> ParticipantIds,
        string? LastMessage,
        Instant? LastMessageAt);
}