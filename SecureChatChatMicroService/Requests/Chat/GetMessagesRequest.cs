namespace Requests.Chat
{
    public record GetMessagesRequest(
        Guid ChatId,
        int Limit,
        int Offset);
}