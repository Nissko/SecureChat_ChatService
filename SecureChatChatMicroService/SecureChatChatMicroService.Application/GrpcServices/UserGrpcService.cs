using Grpc.Core;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtobufMappers;
using UserService.Proto;

namespace SecureChatChatMicroService.Application.GrpcServices
{
    /// <summary>
    /// GRPC-Сервис для UserEntity
    /// </summary>
    public class UserGrpcService
        : UserService.Proto.UserGrpcService.UserGrpcServiceBase
    {
        private readonly IUserRepository _userRepository;

        public UserGrpcService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        /// <summary>
        /// 
        /// </summary>
        public override async Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
        {
            try
            {
                var users = await _userRepository.GetAll();
                return new()
                {
                    Success = true,
                    Users = { users.ToProtoUserInfoList() }
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override async Task<GetUserResponse> GetUser(GetUserRequest request, ServerCallContext context)
        {
            try
            {
                var user = await _userRepository.FromId(request.Id.ToGuid());
                return new()
                {
                    Success = true,
                    Users = user.ToProtoUserInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override async Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
        {
            try
            {
                var newUser =
                    await _userRepository.Create(new(request.UserProfileId.ToGuid()));
                return new()
                {
                    Success = true,
                    Id = newUser.ToString()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
        {
            try
            {
                var updateUser = await _userRepository
                    .Update(new(request.Id.ToGuid(), request.IsDeleted));

                return new()
                {
                    Success = true,
                    UpdateUser = updateUser.ToProtoUserInfo()
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
        {
            try
            {
                _ = await _userRepository.Delete(request.Id.ToGuid());
                return new()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                throw new RpcException(new(StatusCode.Aborted, ex.Message));
            }
        }
    }
}