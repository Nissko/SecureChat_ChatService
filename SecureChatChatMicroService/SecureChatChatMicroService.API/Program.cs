using Microsoft.OpenApi.Models;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.GrpcServices;
using SecureChatChatMicroService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
})
.AddJsonTranscoding();

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChatMicroservice User API",
        Version = "v1",
        Description = "gRPC-сервис чата",
        Contact = new OpenApiContact
        {
            Name = "Nikita",
            Email = "mail@nikita-skibko.ru"
        }
    });

    var xmlPath = Path.Combine(AppContext.BaseDirectory, "SecureChatChatMicroService.xml");
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
        options.IncludeGrpcXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

// Регистрация gRPC-сервисов
app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
// app.MapGrpcService<UserProfileGrpcService>().EnableGrpcWeb();
// app.MapGrpcService<BlockUserGrpcService>().EnableGrpcWeb();

app.Run();