namespace Requests.ChatGroup
{
    public record UpdateChatGroupRequest(
        Guid Id,
        Guid? ChatId,
        Guid? GroupId);
}