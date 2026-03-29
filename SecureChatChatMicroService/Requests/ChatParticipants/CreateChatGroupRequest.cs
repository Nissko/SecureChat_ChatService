namespace Requests.ChatParticipants
{
    public record CreateChatParticipantsRequest(
        Guid ChatId,
        Guid UserId);
}