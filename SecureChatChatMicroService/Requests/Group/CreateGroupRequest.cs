namespace Requests.Group
{
    public record CreateGroupRequest(
        string Name,
        Guid UserId);
}