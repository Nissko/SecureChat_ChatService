using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserEntity
    /// </summary>
    public class UserGrpcService(IUserRepository userRepository) : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {

    }
}