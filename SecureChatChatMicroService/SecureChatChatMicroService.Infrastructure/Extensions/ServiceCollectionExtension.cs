using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecureChatChatMicroService.Application.Extensions;

namespace SecureChatChatMicroService.Infrastructure.Extensions;

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
            
            
            //TODO: Репозитории

            services.AddApplication();

            return services;
        }
    }