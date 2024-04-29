using BlogAPI.Data;
using System.Net.WebSockets;
using System.Text;

namespace BlogAPI.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly BlogDbContext _dbContext;

    public WebSocketMiddleware(RequestDelegate next, BlogDbContext dbContext)
    {
        _next = next;
        _dbContext = dbContext;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await HandleWebSocketConnection(context, webSocket);
        }
        else
        {
            await _next(context);
        }
    }

    private async Task HandleWebSocketConnection(HttpContext context, WebSocket webSocket)
    {
        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                var buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    await SendMessageAsync(webSocket, "Mensagem recebida pelo servidor: " + message);
                }
            }
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine("Exceção de WebSocket: " + ex.Message);

            if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.CloseReceived && webSocket.State != WebSocketState.CloseSent)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Erro no WebSocket", CancellationToken.None);
            }
        }
    }

    private async Task SendMessageAsync(WebSocket webSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}