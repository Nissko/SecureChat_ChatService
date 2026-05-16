using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureChatChatMicroService.Application.Common.Interfaces;
using SecureChatChatMicroService.Application.Common.Interfaces.IRepositories;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.Extensions.ProtoManagers;
using SecureChatChatMicroService.Infrastructure.Repositories;

namespace SecureChatChatMicroService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCollectionInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddDbContext<ChatServiceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSqlDatabase")));

            services.AddScoped<ChatServiceDbContext>(provider => provider.GetService<ChatServiceDbContext>()
                                                                 ?? throw new InvalidOperationException());

            //TODO: Регистрация  Mediator(-a)
            
            services.AddSingleton<ChatConnectionManager>();
            
            //TODO: Репозитории
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatServiceDbContext, ChatServiceDbContext>();

            services.AddApplication();

            return services;
        }
    }
}