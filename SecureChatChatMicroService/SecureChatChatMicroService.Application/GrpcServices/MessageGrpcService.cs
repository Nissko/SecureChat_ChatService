using Grpc.Core;
using MessageService.Proto;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для MessageEntity
    /// </summary>
    public class MessageGrpcService
        : MessageService.Proto.MessageGrpcService.MessageGrpcServiceBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageGrpcService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public override async Task<GetMessageResponse> GetMessage(GetMessageRequest request, ServerCallContext context)
        {
            try
            {
                var message = await _messageRepository.FromId(request.Id.ToGuid());
                return new GetMessageResponse
                {
                    Success = true,
                    Messages = message.ToProtoMessageInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<GetMessagesResponse> GetMessages(GetMessagesRequest request,
            ServerCallContext context)
        {
            try
            {
                var messages = await _messageRepository.GetAll(request.ChatId.ToGuid());
                return new GetMessagesResponse
                {
                    Success = true,
                    Messages = { messages.ToProtoMessageInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<CreateMessageResponse> CreateMessage(CreateMessageRequest request,
            ServerCallContext context)
        {
            try
            {
                var newMessage = await _messageRepository.Create(new(
                    request.AnswerMessageId.ToGuid(), request.ChatId.ToGuid(), request.UserId.ToGuid(),
                    request.TypeOfMessage.ToGuid(), request.Content));
                return new CreateMessageResponse
                {
                    Success = true,
                    Id = newMessage.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<UpdateMessageResponse> UpdateMessage(UpdateMessageRequest request,
            ServerCallContext context)
        {
            try
            {
                var updateMessage = await _messageRepository.Update(
                    new Requests.Message.UpdateMessageRequest(request.Id.ToGuid(), request.Content));
                return new UpdateMessageResponse
                {
                    Success = true,
                    UpdateMessage = updateMessage.ToProtoMessageInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<DeleteMessageResponse> DeleteMessage(DeleteMessageRequest request,
            ServerCallContext context)
        {
            try
            {
                var deleteMessage = await _messageRepository.Delete(request.Id.ToGuid());
                return new DeleteMessageResponse
                {
                    Success = deleteMessage
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}