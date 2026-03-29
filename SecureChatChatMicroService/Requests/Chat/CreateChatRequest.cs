namespace Requests.Chat
{
    public record CreateChatRequest(
        Guid Type,
        Guid OwnerId,
        Guid ChatGroupId);
}