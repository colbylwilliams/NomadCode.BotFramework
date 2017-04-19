using System;

namespace NomadCode.BotFramework
{
    public enum SocketStates : long
    {
        Connecting,
        Open,
        Closing,
        Closed
    }

    public class SocketStateChangedEventArgs : EventArgs
    {
        public SocketStates SocketState { get; set; }

        public SocketStateChangedEventArgs (SocketStates socketState) => SocketState = socketState;
    }
}
