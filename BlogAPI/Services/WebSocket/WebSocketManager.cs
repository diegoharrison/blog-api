using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class WebSocketManager
{
    private readonly ConcurrentBag<WebSocket> _webSockets = new ConcurrentBag<WebSocket>();

    public void AddWebSocket(WebSocket webSocket)
    {
        _webSockets.Add(webSocket);
    }

    public async Task SendToAllAsync(string message)
    {
        var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
        foreach (var webSocket in _webSockets)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, default);
            }
        }
    }
}