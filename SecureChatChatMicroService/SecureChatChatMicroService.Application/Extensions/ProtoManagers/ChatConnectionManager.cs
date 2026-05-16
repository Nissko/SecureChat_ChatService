using System.Collections.Concurrent;
using ChatService.Proto;
using Grpc.Core;

namespace SecureChatChatMicroService.Application.Extensions.ProtoManagers
{
    /// <summary>
    /// TODO: Переделать на интерфейс
    /// </summary>
    public class ChatConnectionManager
    {
        private static readonly Lazy<ChatConnectionManager> _instance =
            new(() => new ChatConnectionManager());

        public static ChatConnectionManager Instance => _instance.Value;

        private readonly
            ConcurrentDictionary<string, ConcurrentDictionary<string, IServerStreamWriter<ChatMessageEvent>>>
            _subscriptions
                = new();

        public void Subscribe(string connectionId, string chatId, IServerStreamWriter<ChatMessageEvent> stream)
        {
            var chatSubs = _subscriptions.GetOrAdd(chatId, _ => new());
            chatSubs[connectionId] = stream;
        }

        public void Unsubscribe(string connectionId, string chatId)
        {
            if (_subscriptions.TryGetValue(chatId, out var chatSubs))
            {
                chatSubs.TryRemove(connectionId, out _);
                if (chatSubs.IsEmpty)
                    _subscriptions.TryRemove(chatId, out _);
            }
        }

        public async Task BroadcastAsync(string chatId, ChatMessageEvent evt)
        {
            if (_subscriptions.TryGetValue(chatId, out var chatSubs))
            {
                var tasks = chatSubs.Values.Select(async stream =>
                {
                    try
                    {
                        await stream.WriteAsync(evt);
                    }
                    catch
                    {
                        // ignored
                    }
                });
                await Task.WhenAll(tasks);
            }
        }
    }
}