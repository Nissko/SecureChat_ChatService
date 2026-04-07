using ChatGroupService.Proto;
using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для ChatGroupEntity
    /// </summary>
    public class ChatGroupGrpcService
        : ChatGroupService.Proto.ChatGroupGrpcService.ChatGroupGrpcServiceBase
    {
        private readonly IChatGroupRepository _chatGroupRepository;

        public ChatGroupGrpcService(IChatGroupRepository chatRepository)
        {
            _chatGroupRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public override async Task<GetChatGroupResponse> GetChatGroup(GetChatGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var chatGroup = await _chatGroupRepository.FromId(request.Id.ToGuid());
                return new()
                {
                    Success = true,
                    ChatGroups = chatGroup.ToProtoChatGroupInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<GetChatGroupsResponse> GetChatGroups(GetChatGroupsRequest request,
            ServerCallContext context)
        {
            try
            {
                var chatGroups = await _chatGroupRepository.GetAll();
                return new()
                {
                    Success = true,
                    ChatGroups = { chatGroups.ToProtoChatGroupInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CreateChatGroupResponse> CreateChatGroup(CreateChatGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var createChatGroup = await _chatGroupRepository.Create(
                    new(request.ChatId.ToGuid(), request.GroupId.ToGuid()));
                return new()
                {
                    Success = true,
                    Id = createChatGroup.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UpdateChatGroupResponse> UpdateChatGroup(UpdateChatGroupRequest request,
            ServerCallContext context)
        {
            try
            {
                var updateChatGroup = await _chatGroupRepository.Update(
                    new(request.Id.ToGuid(), request.ChatId.ToGuid(),
                        request.GroupId.ToGuid()));
                return new()
                {
                    Success = true,
                    UpdateChatGroup = updateChatGroup.ToProtoChatGroupInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}