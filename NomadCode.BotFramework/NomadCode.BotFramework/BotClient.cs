#if __IOS__ || __ANDROID__
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Bot.Connector.DirectLine;
using Microsoft.Rest;

using Newtonsoft.Json;


#if __ANDROID__
using Square.OkHttp3;

using WebSocket = Square.OkHttp3.IWebSocket;
#endif

#if __IOS__
using Foundation;

using Square.SocketRocket;

using WebSocket = Square.SocketRocket.WebSocket;
#endif


namespace NomadCode.BotFramework
{
    public partial class BotClient
#if __ANDROID__
        : WebSocketListener
#endif
    {
        static BotClient _shared;
        public static BotClient Shared => _shared ?? (_shared = new BotClient ());


        static DirectLineClient _directLineClient;
        DirectLineClient directLineClient => _directLineClient ?? (!string.IsNullOrEmpty (conversation?.Token) ? _directLineClient = new DirectLineClient (conversation.Token) : throw new Exception ("must set initial client token"));


        public event EventHandler<string> UserTypingMessageReceived;
        public event EventHandler<SocketStateChangedEventArgs> ReadyStateChanged;
        public event NotifyCollectionChangedEventHandler MessagesCollectionChanged;


        public List<BotMessage> Messages { get; set; } = new List<BotMessage> ();


        public bool Initialized => SocketState == SocketStates.Open && HasValidCurrentUser && conversation != null;

        public Func<string, Task<Conversation>> GetOrUpdateConversationAsync { get; set; }


        WebSocket webSocket;

        Conversation conversation;

        long userTypingTimeStampCache;

        bool attemptingReconnect, closingWebsocketAsync;

        ChannelAccount currentUser => new ChannelAccount (CurrentUserId, CurrentUserName);

        TaskCompletionSource<bool> closeSocketTcs;

        SocketStates _ocketState;
        SocketStates SocketState
        {
#if __ANDROID__
            get => _ocketState;
#elif __IOS__
            get => webSocket != null ? (SocketStates)webSocket.ReadyState : _ocketState;
#endif
            set
            {
                if (_ocketState != value)
                {
                    _ocketState = value;

                    ReadyStateChanged?.Invoke (this, new SocketStateChangedEventArgs (_ocketState));
                }
            }
        }


        BotClient () { }


        public void Reset ()
        {
            ResetCurrentUser ();
            webSocket.Close (1001L, "going away");
            webSocket = null;
            conversation = null;
            _directLineClient = null;
            Messages = new List<BotMessage> ();
            MessagesCollectionChanged?.Invoke (this, new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Reset));
        }


        public async Task ConnectSocketAsync (Func<string, Task<Conversation>> getOrUpdateConversationTask = null)
        {
            try
            {
                if (webSocket == null || SocketState == SocketStates.Closed)
                {
#if DEBUG
                    if (ResetConversation)
                    {
                        Log.Info ("Resetting conversation...");

                        ConversationId = string.Empty;
                    }
#endif
                    if (!HasValidCurrentUser)
                    {
                        throw new InvalidOperationException ("BotClient.CurrentUserId and BotClient.CurrentUserName must have values before connecting");
                    }

                    if (getOrUpdateConversationTask == null && GetOrUpdateConversationAsync == null)
                    {
                        throw new InvalidOperationException ("GetOrUpdateConversationAsync must have a value before calling ConnectSocketAsync, otherwise pass it in as a paramater to ConnectSocketAsync");
                    }

                    if (getOrUpdateConversationTask != null)
                    {
                        GetOrUpdateConversationAsync = getOrUpdateConversationTask;
                    }


                    Log.Info ("Getting conversation from server...");

                    conversation = await GetOrUpdateConversationAsync.Invoke (ConversationId);

                    // reset client so it'll pull new token
                    _directLineClient = null;

                    if (string.IsNullOrEmpty (conversation?.StreamUrl))
                    {
                        Log.Info ($"Starting new conversation...");

                        conversation = await directLineClient.Conversations.StartConversationAsync ();

                        if (!string.IsNullOrEmpty (conversation?.ConversationId))
                        {
                            ConversationId = conversation.ConversationId;
                        }
                    }
                    else
                    {
                        Log.Info ($"Reconnect to conversation {ConversationId}...");

                        conversation = await directLineClient.Conversations.ReconnectToConversationAsync (conversation.ConversationId);

                        var activitySet = await directLineClient.Conversations.GetActivitiesAsync (conversation.ConversationId);

                        handleNewActvitySet (activitySet, false);

                        Messages.Sort ((x, y) => y.CompareTo (x));

                        MessagesCollectionChanged?.Invoke (this, new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Reset));
                    }

                    if (!string.IsNullOrEmpty (conversation?.StreamUrl))
                    {
                        Log.Info ($"[Socket Connecting...] {conversation.StreamUrl}");
#if __ANDROID__
                        // set the ConnectionTimeout to 90 seconds because the bot framework sends and empty message every 60 seconds
                        var httpClient = new OkHttpClient.Builder ().ConnectTimeout (90, Java.Util.Concurrent.TimeUnit.Seconds).Build ();

                        webSocket = httpClient.NewWebSocket (new Request.Builder ().Url (conversation.StreamUrl).Build (), this);
#else
                        webSocket = new WebSocket (new NSUrl (conversation.StreamUrl));

                        webSocket.ReceivedMessage += handleWebSocketReceivedMessage;

                        webSocket.WebSocketClosed += handleWebSocketClosed;

                        webSocket.WebSocketFailed += handleWebSocketFailed;

                        webSocket.WebSocketOpened += handleWebSocketOpened;

                        webSocket.ReceivedPong += handleWebSocketReceivedPong;

                        webSocket.Open ();
#endif
                    }
                }
                else if (SocketState == SocketStates.Open)
                {
                    Log.Info ($"[Socket already open, refreshing token and reconnecting...]");

                    //attemptReconnect = true;

                    //webSocket.Close ();

                    //await ConnectSocketAsync ();
                }

                //ReadyStateChanged?.Invoke (this, new ReadyStateChangedEventArgs (webSocket.ReadyState));
            }
            catch (Exception ex)
            {
                Log.Error (ex.Message);
            }
        }


        public async Task ResetWebsocketAsync ()
        {
            setSocketState (SocketStates.Closing);

            var closed = await closeWebsocketAsync ();

            Log.Debug ($"closed == {closed}");

            if (closed)
            {
                Log.Debug ("Reopening socket...\n");
                await ConnectSocketAsync ();
            }

            Task<bool> closeWebsocketAsync ()
            {
                if (!closeSocketTcs.IsNullFinishCanceledOrFaulted ())
                {
                    return closeSocketTcs.Task;
                }

                closeSocketTcs = new TaskCompletionSource<bool> ();

                closingWebsocketAsync = true;

                webSocket.Close (1001L, "going away");

                return closeSocketTcs.Task;
            }
        }


        #region WebSocket Event Handlers


        void handleOpen ()
        {
            Log.Info ($"[Socket Connected] {conversation?.StreamUrl}");

            setSocketState (SocketStates.Open);
        }


        void handleClosing ()
        {
            Log.Info ($"[Socket Closing] {conversation?.StreamUrl}");

            setSocketState (SocketStates.Closing);
        }


        void handleClosed (int code, string reason)
        {
            Log.Info ($"[Socket Disconnected] Reason: {reason}  Code: {code}");

            if (closingWebsocketAsync && !closeSocketTcs.IsNullFinishCanceledOrFaulted ())
            {
                closingWebsocketAsync = false;

                Log.Debug ($"e.Code: {code}");
                Log.Debug ($"e.Reason: {reason}");

                if (!closeSocketTcs.TrySetResult (true))
                {
                    Log.Error ("Failed to set closeSocketTcs result");
                }
            }

            setSocketState (SocketStates.Closed);
        }


        void handleFailure (string message, int? code, string stacktrace)
        {
            Log.Info ($"[Socket Failed to Connect] Error: {message}  Code: {code}");
            Log.Info ($"[Socket Failed to Connect] Error: {stacktrace}");
        }


        void handleMessage (string message)
        {
            if (string.IsNullOrEmpty (message)) // Ignore empty messages
            {
                Log.Info ($"[Socket Message Received] Empty message, ignoring");

                return;
            }

            //Log.Info ($"[Socket Message Received] \n{message}");

            var activitySet = JsonConvert.DeserializeObject<ActivitySet> (message);

            handleNewActvitySet (activitySet);
        }

        #endregion


        void handleNewActvitySet (ActivitySet activitySet, bool changedEvents = true)
        {
            //var watermark = activitySet?.Watermark;

            var activities = activitySet?.Activities;

            if (activities != null)
            {
                foreach (var activity in activities)
                {
                    switch (activity.Type)
                    {
                        case ActivityTypes.Message:

                            var newMessage = new BotMessage (activity);

                            var message = Messages.FirstOrDefault (m => m.Equals (newMessage));

                            if (message != null)
                            {
                                //Log.Debug ($"Updating Existing Message: {activity.TextFormat} :: {activity.Text}");

                                message.Update (activity);

                                if (changedEvents)
                                {
                                    Messages.Sort ((x, y) => y.CompareTo (x));

                                    var index = Messages.IndexOf (message);

                                    MessagesCollectionChanged?.Invoke (this, new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Replace, message, message, index));
                                }
                            }
                            else
                            {
                                Messages.Insert (0, newMessage);

                                if (changedEvents)
                                {
                                    MessagesCollectionChanged?.Invoke (this, new NotifyCollectionChangedEventArgs (NotifyCollectionChangedAction.Add, message));
                                }
                            }

                            break;
                        case ActivityTypes.ContactRelationUpdate:
                            break;
                        case ActivityTypes.ConversationUpdate:
                            break;
                        case ActivityTypes.Typing:

                            if (activity?.From.Id != CurrentUserId && !string.IsNullOrEmpty (activity?.From?.Name))
                            {
                                UserTypingMessageReceived?.Invoke (this, activity.From.Name);
                            }

                            break;
                        case ActivityTypes.Ping:
                            break;
                        case ActivityTypes.EndOfConversation:
                            break;
                        case ActivityTypes.Trigger:
                            break;
                    }
                }
            }
        }


        public bool SendMessage (string text)
        {
            var activity = new Activity
            {
                From = currentUser,
                Text = text,
                Type = ActivityTypes.Message,
                LocalTimestamp = DateTimeOffset.Now,
                Timestamp = DateTime.UtcNow
            };

            var message = new BotMessage (activity);

            var posted = postActivityAsync (activity);

            if (posted)
            {
                Messages.Insert (0, message);
            }

            return posted;
        }


        public void SendUserTyping ()
        {
            const long delayTicks = TimeSpan.TicksPerSecond * 3;

            if (attemptingReconnect) return;

            var utcNowTicks = DateTime.UtcNow.Ticks;

            if (userTypingTimeStampCache == 0 || utcNowTicks - userTypingTimeStampCache > delayTicks)
            {
                userTypingTimeStampCache = utcNowTicks;

                Log.Debug ("Sending User Typing");

                var activity = new Activity
                {
                    From = currentUser,
                    Type = ActivityTypes.Typing
                };

                postActivityAsync (activity, true);
            }
        }


        bool postActivityAsync (Activity activity, bool ignoreFailure = false)
        {
            if (conversation == null)
            {
                if (ignoreFailure) return false;

                throw new ArgumentNullException (nameof (conversation), "cannot be null to send message");
            }

            if (!Initialized)
            {
                if (ignoreFailure) return false;

                Log.Error ("client is not properly initialized");

                return false;
                //throw new Exception ("client is not properly initialized");
            }

            Task.Run (async () =>
            {
                try
                {
                    if (attemptingReconnect)
                    {
                        Log.Debug ($"attemptingReconnect == {attemptingReconnect} - returning");
                        return;
                    }

                    await directLineClient.Conversations.PostActivityAsync (conversation.ConversationId, activity).ConfigureAwait (false);

                    attemptingReconnect = false;
                }
                catch (HttpOperationException httpEx)
                {
                    Log.Error (httpEx.Message);

                    if (httpEx.Response.StatusCode == HttpStatusCode.Forbidden && !attemptingReconnect)
                    {
                        Log.Debug ($"attemptingReconnect == {attemptingReconnect}");

                        attemptingReconnect = true;

                        await ResetWebsocketAsync ();

                        await directLineClient.Conversations.PostActivityAsync (conversation.ConversationId, activity).ConfigureAwait (false);

                        attemptingReconnect = false;
                    }

                    else throw;
                }
                catch (Exception ex)
                {
                    Log.Error (ex.Message);

                    if (!ignoreFailure)
                        throw;
                }
            });

            return true;
        }


        public void SendPing ()
        {
            if (!Initialized) return;

            Log.Debug ("Sending Ping...");

            webSocket.SendPing ();
        }


#if __ANDROID__

        void setSocketState (SocketStates state) => SocketState = state;

        public override void OnOpen (IWebSocket webSocket, Response response)
        {
            base.OnOpen (webSocket, response);
            handleOpen ();
        }

        public override void OnMessage (IWebSocket webSocket, string text)
        {
            base.OnMessage (webSocket, text);
            handleMessage (text);
        }

        public override void OnClosing (IWebSocket webSocket, int code, string reason)
        {
            base.OnClosing (webSocket, code, reason);
            handleClosing ();
        }

        public override void OnClosed (IWebSocket webSocket, int code, string reason)
        {
            base.OnClosed (webSocket, code, reason);
            handleClosed (code, reason);
        }

        public override void OnFailure (IWebSocket webSocket, Java.Lang.Throwable t, Response response)
        {
            base.OnFailure (webSocket, t, response);
            handleFailure (t?.LocalizedMessage, response?.Code (), t?.StackTrace);
        }

#elif __IOS__

        void setSocketState (SocketStates state) => SocketState = webSocket != null ? (SocketStates)webSocket.ReadyState : state;

        void handleWebSocketOpened (object sender, EventArgs e)
        {
            handleOpen ();
        }

        void handleWebSocketReceivedMessage (object sender, WebSocketReceivedMessageEventArgs e)
        {
            handleMessage (e.Message.ToString ());
        }

        void handleWebSocketClosed (object sender, WebSocketClosedEventArgs e)
        {
            handleClosed ((int)e.Code, e.Reason);
        }

        void handleWebSocketFailed (object sender, WebSocketFailedEventArgs e)
        {
            handleFailure (e.Error?.LocalizedDescription, (int?)e.Error?.Code, e.Error?.ToString ());
        }

        void handleWebSocketReceivedPong (object sender, WebSocketReceivedPongEventArgs e)
        {
            Log.Info ($"[Socket Received Pong] {Environment.NewLine}");
        }

#endif

    }
}

#endif