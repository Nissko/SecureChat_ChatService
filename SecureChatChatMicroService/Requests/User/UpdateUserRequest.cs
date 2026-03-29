namespace Requests.User
{
    public record UpdateUserRequest(
        Guid Id,
        Guid? UserProfileId,
        bool? IsDeleted);
}