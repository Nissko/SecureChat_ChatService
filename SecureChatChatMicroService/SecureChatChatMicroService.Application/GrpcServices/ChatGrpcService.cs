using ChatService.Proto;
using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для ChatEntity
    /// </summary>
    public class ChatGrpcService
        : ChatService.Proto.ChatGrpcService.ChatGrpcServiceBase
    {
        private readonly IChatRepository _chatRepository;

        public ChatGrpcService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        public override async Task<GetChatResponse> GetChat(GetChatRequest request, ServerCallContext context)
        {
            try
            {
                var chat = await _chatRepository.FromId(request.Id.ToGuid());
                return new()
                {
                    Success = true,
                    Chats = chat.ToProtoChatInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<GetChatsResponse> GetChats(GetChatsRequest request, ServerCallContext context)
        {
            try
            {
                var chats = await _chatRepository.GetAll();
                return new()
                {
                    Success = true,
                    Chats = { chats.ToProtoChatInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CreateChatResponse> CreateChat(CreateChatRequest request, ServerCallContext context)
        {
            try
            {
                var createChat = await _chatRepository.Create(new(request.Type.ToGuid(),
                    request.OwnerId.ToGuid(), request.ChatGroupId.ToGuid()));
                return new()
                {
                    Success = true,
                    Id = createChat.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UpdateChatResponse> UpdateChat(UpdateChatRequest request, ServerCallContext context)
        {
            try
            {
                var updateChat = await _chatRepository.Update(new(request.Id.ToGuid(),
                    request.LastMessageTime.ToInstant(), request.CountUnreadMessages, request.IsPint, request.IsMute,
                    request.Type.ToGuid(), request.GroupId.ToGuid()));
                return new()
                {
                    Success = true,
                    UpdateChat = updateChat.ToProtoChatInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<DeleteChatResponse> DeleteChat(DeleteChatRequest request, ServerCallContext context)
        {
            try
            {
                var deleteChat = await _chatRepository.Delete(request.Id.ToGuid());
                return new()
                {
                    Success = deleteChat
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}