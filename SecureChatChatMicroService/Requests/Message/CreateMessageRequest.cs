namespace Requests.Message
{
    public record CreateMessageRequest(
        Guid AnswerMessageId,
        Guid ChatId,
        Guid UserId,
        Guid TypeOfMessage,
        string Content);
}