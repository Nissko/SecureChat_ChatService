using Dtos.DTO.GroupDtos.Crud;

namespace Dtos.DTO.UserDtos.Crud
{
    public record UserDto(
        Guid Id,
        Guid UserProfileId,
        bool IsDeleted,
        IEnumerable<GroupDto> Groups);
}