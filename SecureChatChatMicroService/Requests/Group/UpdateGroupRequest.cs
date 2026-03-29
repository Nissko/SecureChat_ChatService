namespace Requests.Group
{
    public record UpdateGroupRequest(
        Guid Id,
        string? Name,
        Guid? UserId);
}