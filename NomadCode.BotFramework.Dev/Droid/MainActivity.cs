using Android.App;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System;

using NomadCode.BotFramework;

namespace NomadCode.BotFramework.Dev.Droid
{
	[Activity (Label = "NomadCode.BotFramework.Dev", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Android.App.Activity
	{
		//int count = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			try
			{
				if (!string.IsNullOrEmpty (Keys.Bot.DirectLineSecret))
				{
					BotClient.Shared.CurrentUserId = "userId";
					BotClient.Shared.CurrentUserName = "User";
					BotClient.Shared.CurrentUserEmail = "hello@xamarin.com";
					//BotClient.Shared.SetAvatarUrl (userId, avatarUrl);

					if (!BotClient.Shared.Initialized)
					{
						var client = new DirectLineClient (Keys.Bot.DirectLineSecret);

						Task.Run (async () =>
						{
							try
							{
								BotClient.Shared.ReadyStateChanged += async (sender, e) =>
								{
									if (e.SocketState == SocketStates.Open)
									{
										var count = 0;

										while (count < 10)
										{
											count++;

											switch (count)
											{
												case 2:
													BotClient.Shared.SendMessage ($"Hero card");
													break;
												case 3:
													BotClient.Shared.SendMessage ($"Thumbnail card");
													break;
												case 4:
													BotClient.Shared.SendMessage ($"Receipt card");
													break;
												case 5:
													BotClient.Shared.SendMessage ($"Sign-in card");
													break;
												case 6:
													BotClient.Shared.SendMessage ($"Animation card");
													break;
												case 7:
													BotClient.Shared.SendMessage ($"Video card");
													break;
												case 8:
													BotClient.Shared.SendMessage ($"Audio card");
													break;
												default:
													BotClient.Shared.SendMessage ($"Test {count}");
													break;
											}

											await Task.Delay (15000);
										}
									}
								};

								await BotClient.Shared.ConnectSocketAsync (conversationId => client.Tokens.GenerateTokenForNewConversationAsync ());

							}
							catch (Exception ex)
							{
								Log.Error (ex.Message);
							}
						});
					}
				}
				else
				{
					Log.Debug ("To use the Bot, add your bot's direct line secret to Keys.Bot.DirectLineSecret");
				}
			}
			catch (Exception ex)
			{
				Log.Error (ex.Message);
			}
			// Get our button from the layout resource,
			// and attach an event to it
			//Button button = FindViewById<Button> (Resource.Id.myButton);

			//button.Click += delegate { button.Text = $"{count++} clicks!"; };
		}
	}
}

