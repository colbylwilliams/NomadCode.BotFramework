#if __IOS__ || __ANDROID__

namespace NomadCode.BotFramework
{
    public static class SocketExtensions
    {
#if __IOS__

        public static void Close (this Square.SocketRocket.WebSocket webSocket, long code, string reason) => webSocket.Close ((Square.SocketRocket.StatusCode)code, reason);

#elif __ANDROID__

        public static void Close (this Square.OkHttp3.IWebSocket webSocket, long code, string reason) => webSocket.Close ((int)code, reason);

        public static void SendPing (this Square.OkHttp3.IWebSocket webSocket) => throw new System.NotImplementedException ();

#endif
    }
}

#endif