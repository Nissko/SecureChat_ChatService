using NodaTime;

namespace Requests.Message
{
    public record SendMessageRequest(
        Guid ChatId,
        Guid UserId,
        Guid? AnswerMessageId,
        string Text,
        Instant SendTime);
}