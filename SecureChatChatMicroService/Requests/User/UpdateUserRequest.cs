using NodaTime;

namespace Requests.User
{
    public record UpdateUserRequest(
        Guid Id,
        Instant? IsDeleted);
}