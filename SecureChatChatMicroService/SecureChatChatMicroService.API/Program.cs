using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SecureChatChatMicroService.Application.Extensions;
using SecureChatChatMicroService.Infrastructure.Extensions;
using ChatGrpcService = SecureChatChatMicroService.Application.GrpcServices.ChatGrpcService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    })
    .AddJsonTranscoding();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(
        IPAddress.Any,
        5576,
        listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });

    // HTTP endpoint — порт 4127 (для Swagger, Health, REST)
    options.Listen(
        IPAddress.Any,
        4127,
        listenOptions =>
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

app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    service = "ChatChatService"
}));

// Регистрация gRPC-сервисов
app.MapGrpcService<ChatGrpcService>().EnableGrpcWeb();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client...");

app.Run();