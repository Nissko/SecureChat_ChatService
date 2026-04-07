namespace Requests.User
{
    public record UpdateUserRequest(
        Guid Id,
        bool? IsDeleted);
}