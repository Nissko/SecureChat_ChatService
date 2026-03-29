using NodaTime;

namespace Requests.Chat
{
    public record UpdateChatRequest(
        Guid Id,
        Instant? LastMessageTime,
        int? CountUnreadMessages,
        bool? IsPint,
        bool? IsMute,
        Guid? Type,
        Guid? GroupId);
}