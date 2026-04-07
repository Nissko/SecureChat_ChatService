using GroupService.Proto;
using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserEntity
    /// </summary>
    public class GroupGrpcService
        : GroupService.Proto.GroupGrpcService.GroupGrpcServiceBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupGrpcService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public override async Task<GetGroupResponse> GetGroup(GetGroupRequest request, ServerCallContext context)
        {
            try
            {
                var group = await _groupRepository.FromId(request.Id.ToGuid());
                return new()
                {
                    Success = true,
                    Groups = group.ToProtoGroupInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<GetGroupsResponse> GetGroups(GetGroupsRequest request, ServerCallContext context)
        {
            try
            {
                var groups = await _groupRepository.GetAll();
                return new()
                {
                    Success = true,
                    Groups = { groups.ToProtoGroupInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CreateGroupResponse> CreateGroup(CreateGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var newGroup = await _groupRepository.Create(new(
                    request.Name, request.UserId.ToGuid()));
                return new()
                {
                    Success = true,
                    Id = newGroup.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UpdateGroupResponse> UpdateGroup(UpdateGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var updateGroup = await _groupRepository.Update(
                    new(request.Id.ToGuid(), request.Name, request.UserId.ToGuid()));
                return new()
                {
                    Success = true,
                    UpdateGroup = updateGroup.ToProtoGroupInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<DeleteGroupResponse> DeleteGroup(DeleteGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var deleteGroup = await _groupRepository.Delete(request.Id.ToGuid());
                return new()
                {
                    Success = deleteGroup
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}