using NodaTime;

namespace Requests.Message
{
    public record SendMessageRequest(
        Guid ChatId,
        Guid ChatParticipantId,
        Guid? AnswerMessageId,
        string Text,
        Instant SendTime);
}