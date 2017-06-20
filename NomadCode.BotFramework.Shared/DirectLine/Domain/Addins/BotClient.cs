#if __IOS__ || __ANDROID__
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
using Newtonsoft.Json.Linq;
using System.IO;
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


		public event EventHandler<string> ForceSendMessage;
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

			handleNewActvitySet (activitySet, true, message);
		}

		#endregion

		void handleNewActvitySet (ActivitySet activitySet, bool changedEvents = true, string json = null)
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

							const string strongOpen = "<strong>";
							const string strongClose = "</strong>";
							const string userTyping = "User is typing...";
							const string strongReplace = "**";
							const string randomSpace = "\n                  ";

							// LITWARE: replace "<strong>text</strong>" with markdown: "**text**" per link below
							// https://docs.microsoft.com/en-us/bot-framework/rest-api/bot-framework-rest-connector-create-messages
							if (newMessage.HasText && newMessage.Activity.Text.Contains (strongOpen))
							{
								newMessage.Activity.Text = newMessage.Activity.Text.Replace (randomSpace, " ");
								newMessage.Activity.Text = newMessage.Activity.Text.Replace (strongOpen, strongReplace);
								newMessage.Activity.Text = newMessage.Activity.Text.Replace (strongClose, strongReplace);
							}

							if (newMessage.HasAtachments)
							{
								foreach (var attachment in newMessage.Attachments)
								{
									if (attachment.Content.HasText && attachment.Content.Text.Contains (strongOpen))
									{
										attachment.Content.Text = attachment.Content.Text.Replace (strongOpen, strongReplace);
										attachment.Content.Text = attachment.Content.Text.Replace (strongClose, strongReplace);
									}
								}
							}

							if (string.Compare (CurrentUserName, DefaultUserName, true) == 0 && !string.IsNullOrEmpty (newMessage.LitwareChannelData?.AccountId))
							{
								CurrentUserId = newMessage.LitwareChannelData.Username;
								CurrentUserName = newMessage.LitwareChannelData.Username;
								SetAvatarUrl (CurrentUserId, $"https://cisbot-prod.azurewebsites.net/api/opportunity/images/account?id={newMessage.LitwareChannelData.AccountId}");
								// https://cisbot-prod.azurewebsites.net/api/opportunity/images/account?id=
								// https://cisbot-prod.azurewebsites.net/assets/images/chat_person_120x120.png
							}

							// LITWARE: ignore bs messages returned by the litware bot
							if (!(newMessage.HasText && string.Compare (newMessage.Activity.Text, userTyping, true) == 0))
							{
								if (!string.IsNullOrEmpty (json))
									Log.Debug ($"[Socket Message Received]\nMessage:\n{json}");

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
							}
							else
							{
								if (!string.IsNullOrEmpty (json))
									Log.Debug ($"[Socket Message Received] Message: Litware junk");
							}
							break;
						case ActivityTypes.ContactRelationUpdate:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received]\nContactRelationUpdate:\n{json}");

							break;
						case ActivityTypes.ConversationUpdate:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received]\nConversationUpdate:\n{json}");

							break;
						case ActivityTypes.Typing:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received] Typing");
							//Log.Debug ($"[Socket Message Received]\nTyping:\n{json}");

							if (activity?.From.Id != CurrentUserId && !string.IsNullOrEmpty (activity?.From?.Name))
							{
								UserTypingMessageReceived?.Invoke (this, activity.From.Name);
							}

							break;
						case ActivityTypes.Ping:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received]\nPing:\n{json}");

							break;
						case ActivityTypes.EndOfConversation:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received]\nEndOfConversation:\n{json}");

							break;
						case ActivityTypes.Trigger:

							if (!string.IsNullOrEmpty (json))
								Log.Debug ($"[Socket Message Received]\nTrigger:\n{json}");

							break;
					}
				}
			}
		}


		public void HandleCardAction (CardAction action)
		{
			if (!string.IsNullOrEmpty (action?.Type))
			{
				switch (action.Type)
				{
					case ActionTypes.OpenUrl:

						break;
					case ActionTypes.ImBack:

						ForceSendMessage?.Invoke (this, action.Value);

						break;
					case ActionTypes.PostBack:

						break;
					case ActionTypes.Call:

						break;
					case ActionTypes.PlayAudio:

						break;
					case ActionTypes.PlayVideo:

						break;
					case ActionTypes.ShowImage:

						break;
					case ActionTypes.DownloadFile:

						break;
					case ActionTypes.Signin:

						break;
					default:
						break;
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

				//postActivityAsync (activity, true);
				Log.Debug ("Not sending Typing Activity");
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


		public async Task<bool> SendUploadAsync (Stream stream)
		{
			if (conversation == null)
			{
				throw new ArgumentNullException (nameof (conversation), "cannot be null to send message");
			}

			if (!Initialized)
			{
				Log.Error ("client is not properly initialized");

				return false;
				//throw new Exception ("client is not properly initialized");
			}

			var activity = new Activity
			{
				From = currentUser,
				Text = " ",
				Type = ActivityTypes.Message,
				LocalTimestamp = DateTimeOffset.Now,
				Timestamp = DateTime.UtcNow,
				ReplyToId = Messages.FirstOrDefault (m => m.Activity.From.Id != currentUser.Id)?.Activity.Id
			};

			//ForceSendMessage?.Invoke (this, "Image");

			try
			{
				if (attemptingReconnect)
				{
					Log.Debug ($"attemptingReconnect == {attemptingReconnect} - returning");
					return false;
				}

				ResourceResponse resourceResponse = await directLineClient.Conversations.UploadLitwareAsync (conversation.ConversationId, stream, currentUser.Id, activity);


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

					await directLineClient.Conversations.UploadLitwareAsync (conversation.ConversationId, stream, currentUser.Id, activity);

					attemptingReconnect = false;
				}

				else throw;
			}
			catch (Exception ex)
			{
				Log.Error (ex.Message);
				throw;

			}

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