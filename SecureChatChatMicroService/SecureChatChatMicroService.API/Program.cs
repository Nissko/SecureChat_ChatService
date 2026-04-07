using Microsoft.AspNetCore.Server.Kestrel.Core;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Application.GrpcServices;
using SecureChatChatMicroService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    })
    .AddJsonTranscoding();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5555, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });

    options.ListenLocalhost(5277, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

builder.Services
    .AddCollectionInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ChatMicroservice Chat API",
        Version = "v1",
        Description = "gRPC-сервис чата",
        Contact = new()
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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.EnableTryItOutByDefault();
    });
}

// Регистрация gRPC-сервисов
app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<GroupGrpcService>().EnableGrpcWeb();
app.MapGrpcService<ChatGroupGrpcService>().EnableGrpcWeb();
app.MapGrpcService<ChatGrpcService>().EnableGrpcWeb();
app.MapGrpcService<ChatParticipantsGrpcService>().EnableGrpcWeb();
app.MapGrpcService<MessageGrpcService>().EnableGrpcWeb();

app.Run();