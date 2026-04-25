FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SecureChatChatMicroService.API/SecureChatChatMicroService.API.csproj", "SecureChatChatMicroService.API/"]
COPY ["Protos/Protos.csproj", "Protos/"]
COPY ["SecureChatChatMicroService.Infrastructure/SecureChatChatMicroService.Infrastructure.csproj", "SecureChatChatMicroService.Infrastructure/"]
COPY ["SecureChatChatMicroService.Application/SecureChatChatMicroService.Application.csproj", "SecureChatChatMicroService.Application/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["Requests/Requests.csproj", "Requests/"]
COPY ["SecureChatChatMicroService.Domain/SecureChatChatMicroService.Domain.csproj", "SecureChatChatMicroService.Domain/"]
RUN dotnet restore "SecureChatChatMicroService.API/SecureChatChatMicroService.API.csproj"
COPY . .
WORKDIR "/src/SecureChatChatMicroService.API"
RUN dotnet build "./SecureChatChatMicroService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SecureChatChatMicroService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecureChatChatMicroService.API.dll"]
