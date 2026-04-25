namespace Requests.Chat
{
    public record GetUserChatsRequest(
        Guid UserId,
        int Limit,
        int Offset);
}