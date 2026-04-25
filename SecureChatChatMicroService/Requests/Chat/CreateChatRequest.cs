namespace Requests.Chat
{
    public record CreateChatRequest(
        List<Guid> ParticipantIds,
        Guid? CreatedBy);
}