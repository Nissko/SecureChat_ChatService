using ChatParticipantsService.Proto;
using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserEntity
    /// </summary>
    public class ChatParticipantsGrpcService
        : ChatParticipantsService.Proto.ChatParticipantsGrpcService.ChatParticipantsGrpcServiceBase
    {
        private readonly IChatParticipantsRepository _chatParticipantsRepository;

        public ChatParticipantsGrpcService(IChatParticipantsRepository chatParticipantsRepository)
        {
            _chatParticipantsRepository = chatParticipantsRepository ??
                                          throw new ArgumentNullException(nameof(chatParticipantsRepository));
        }

        public override async Task<GetChatParticipantResponse> GetChatParticipant(GetChatParticipantRequest request,
            ServerCallContext context)
        {
            try
            {
                var chatParticipant = await _chatParticipantsRepository.FromId(request.Id.ToGuid());
                return new()
                {
                    Success = true,
                    ChatParticipants = chatParticipant.ToProtoChatParticipantsInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<GetChatParticipantsResponse> GetChatParticipants(GetChatParticipantsRequest request,
            ServerCallContext context)
        {
            try
            {
                var chatParticipants = await _chatParticipantsRepository.GetAll();
                return new()
                {
                    Success = true,
                    ChatParticipants = { chatParticipants.ToProtoChatParticipantsInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CreateChatParticipantsResponse> CreateChatParticipants(
            CreateChatParticipantsRequest request, ServerCallContext context)
        {
            try
            {
                var createChatParticipants = await _chatParticipantsRepository.Create(
                    new(request.ChatId.ToGuid(),
                        request.UserId.ToGuid()));
                return new()
                {
                    Success = true,
                    Id = createChatParticipants.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UpdateChatParticipantsResponse> UpdateChatParticipants(
            UpdateChatParticipantsRequest request, ServerCallContext context)
        {
            try
            {
                var updateChatParticipants =
                    await _chatParticipantsRepository.Update(new(request.Id.ToGuid(), request.ExitTime.ToInstant()));
                return new()
                {
                    Success = true,
                    UpdateChatParticipants = updateChatParticipants.ToProtoChatParticipantsInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<DeleteChatParticipantsResponse> DeleteChatParticipants(
            DeleteChatParticipantsRequest request, ServerCallContext context)
        {
            try
            {
                var deleteChatParticipants = await _chatParticipantsRepository.Delete(request.Id.ToGuid());
                return new()
                {
                    Success = deleteChatParticipants
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}