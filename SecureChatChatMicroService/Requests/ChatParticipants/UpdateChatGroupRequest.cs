using NodaTime;

namespace Requests.ChatParticipants
{
    public record UpdateChatParticipantsRequest(
        Guid Id,
        Instant? ExitTime);
}