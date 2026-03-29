using NodaTime;

namespace Dtos.DTO.MessageDtos.Crud
{
    public record MessageDto(
        Guid Id,
        Guid? AnswerMessageId,
        Guid ChatId,
        Guid UserId,
        Guid TypeOfMessage,
        string Content,
        Instant SendTime,
        Instant? UpdateTime,
        Instant? DeleteTime,
        bool IsEdited,
        bool IsDeleted);
}