using Dtos.DTO.ChatGroupDtos.Crud;

namespace Dtos.DTO.GroupDtos.Crud
{
    public record GroupDto(
        Guid Id,
        string Name,
        Guid UserId,
        IEnumerable<ChatGroupDto> ChatGroups);
}