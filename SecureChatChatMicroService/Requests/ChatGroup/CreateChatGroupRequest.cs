namespace Requests.ChatGroup
{
    public record CreateChatGroupRequest(
        Guid ChatId,
        Guid GroupId);
}