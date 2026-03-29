namespace Requests.Message
{
    public record UpdateMessageRequest(
        Guid Id,
        string? Content);
}