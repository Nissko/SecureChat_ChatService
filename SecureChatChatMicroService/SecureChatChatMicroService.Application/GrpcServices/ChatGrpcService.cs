using ChatService.Proto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;
using SecureChatChatMicroService.Application.Extensions.ProtoManagers;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для ChatEntity
    /// </summary>
    public class ChatGrpcService
        : ChatService.Proto.ChatGrpcService.ChatGrpcServiceBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public ChatGrpcService(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public override async Task<ChatResponse> CreateChat(CreateChatRequest request, ServerCallContext context)
        {
            try
            {
                var participantIds = request.ParticipantIds.Select(x => x.ToGuid()).ToList();
                if (participantIds.Count == 0)
                {
                    throw new Exception("No participantIds provided");
                }
                
                var newChat = await _chatRepository.CreateChat(new(participantIds, null));
                return new ChatResponse
                {
                    Id = newChat.Id.ToString(),
                    LastMessage = newChat.LastMessage ?? "",
                    LastMessageAt = newChat.LastMessageAt?.ToTimestamp() ?? null,
                    ParticipantIds = { newChat.ParticipantIds.Select(x => x.ToString()).ToList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<ChatsListResponse> GetUserChats(GetUserChatsRequest request,
            ServerCallContext context)
        {
            try
            {
                var userChats = await _chatRepository
                    .GetUserChats(new(request.UserId.ToGuid(), request.Limit, request.Offset));
                return new ChatsListResponse
                {
                    Chats = { userChats.Items.ToProtoChatInfoList() },
                    Total = userChats.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<MessagesListResponse> GetMessages(GetMessagesRequest request,
            ServerCallContext context)
        {
            try
            {
                var messages = await _chatRepository
                    .GetMessages(new(request.ChatId.ToGuid(), request.Limit, request.Offset));

                return new MessagesListResponse
                {
                    Messages = { messages.Items.ToProtoChatMessagesInfoList() },
                    Total = messages.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<ChatResponse> GetChatInfo(GetChatInfoRequest request, ServerCallContext context)
        {
            try
            {
                var chatInfo = await _chatRepository.GetChatInfo(new(request.ChatId.ToGuid()));
                return new ChatResponse
                {
                    Id = chatInfo.Id.ToString(),
                    LastMessage = chatInfo.LastMessage ?? "",
                    LastMessageAt = chatInfo.LastMessageAt?.ToTimestamp() ?? null,
                    ParticipantIds = { chatInfo.ParticipantIds.Select(x => x.ToString()).ToList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<Empty> SendMessage(SendMessageRequest request, ServerCallContext context)
        {
            try
            {
                await _chatRepository.SendMessage(new Requests.Message.SendMessageRequest(
                    request.ChatId.ToGuid(), request.ChatParticipantId.ToGuid(), request.AnswerMessageId.ToGuid(),
                    request.Text, request.SendTime.ToInstant()));

                return new Empty();
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<Message> SendMessageStream(SendMessageRequest request, ServerCallContext context)
        {
            try
            {
                var message = await _chatRepository.SendMessageStream(new Requests.Message.SendMessageRequest(
                    request.ChatId.ToGuid(), request.ChatParticipantId.ToGuid(), request.AnswerMessageId.ToGuid(),
                    request.Text, request.SendTime.ToInstant()));

                return new Message
                {
                    Id = message.Id.ToString(),
                    AnswerMessageId = message.AnswerMessageId.ToString(),
                    ChatId = message.ChatId.ToString(),
                    ChatParticipantId = message.ChatParticipantId.ToString(),
                    TextMessage = message.TextMessage,
                    Timestamp = message.Timestamp.ToTimestamp()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser = await _userRepository.AddUser(request.UserId.ToGuid());
                return new AddUserResponse
                {
                    Success = newUser
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        public override async Task<RemoveUserResponse> RemoveUser(RemoveUserRequest request, ServerCallContext context)
        {
            try
            {
                var removedUser = await _userRepository.RemoveUser(request.UserId.ToGuid());
                return new RemoveUserResponse
                {
                    Success = removedUser
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
        
        public override async Task StreamChatMessages(
            IAsyncStreamReader<ChatMessageRequest> requestStream,
            IServerStreamWriter<ChatMessageEvent> responseStream,
            ServerCallContext context)
        {
            string? subscribedChatId = null;
            string? userId = null;

            // Хранение активных подключений (если надо)
            var connectionId = Guid.NewGuid().ToString();
    
            try
            {
                await foreach (var request in requestStream.ReadAllAsync(context.CancellationToken))
                {
                    switch (request.ActionCase)
                    {
                        case ChatMessageRequest.ActionOneofCase.Subscribe:
                            subscribedChatId = request.Subscribe.ChatId;
                            userId = request.Subscribe.UserId;
                            ChatConnectionManager.Instance.Subscribe(connectionId, subscribedChatId, responseStream);
                            break;
                    
                        case ChatMessageRequest.ActionOneofCase.SendMessage:
                            var newMessage = await SendMessageStream(request.SendMessage, context);

                            await ChatConnectionManager.Instance.BroadcastAsync(
                                request.SendMessage.ChatId,
                                new ChatMessageEvent { NewMessage = newMessage });
                            break;
                    
                        case ChatMessageRequest.ActionOneofCase.Unsubscribe:
                            ChatConnectionManager.Instance.Unsubscribe(connectionId, request.Unsubscribe.ChatId);
                            return;
                    }
                }
            }
            finally
            {
                if (subscribedChatId != null)
                    ChatConnectionManager.Instance.Unsubscribe(connectionId, subscribedChatId);
            }
        }
    }
}