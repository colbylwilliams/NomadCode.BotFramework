#if __IOS__ || __ANDROID__

using System;
using Microsoft.Bot.Connector.DirectLine;

#if __IOS__
using Foundation;
#endif

namespace NomadCode.BotFramework
{
	public class BotMessage : IComparable<BotMessage>, IEquatable<BotMessage>
	{
#if __IOS__
        public nfloat CellHeight { get; set; }

        NSAttributedString _attributedText;

        public NSAttributedString AttributedText => _attributedText ?? (_attributedText = Activity?.Text?.GetMessageAttributedString ());

#elif __ANDROID__
		public float CellHeight { get; set; }
#endif

		public bool Head { get; set; }

		public Activity Activity { get; private set; }

		public int ButtonCount { get; }

		//public bool HasButtons => Activity.Attachments.

		// TODO: cache this and reset when activity is updated
		public DateTime? LocalTimeStamp => Activity.Timestamp?.ToLocalTime () ?? Activity.LocalTimestamp?.LocalDateTime;


		public BotMessage (Activity activity) => Activity = activity;


		public int CompareTo (BotMessage other)
		{
			if (!string.IsNullOrEmpty (Activity?.Id) && !string.IsNullOrEmpty (other?.Activity?.Id))
			{
				return Activity.Id.CompareTo (other.Activity.Id);
			}

			return string.IsNullOrEmpty (Activity?.Id) ? 1 : -1;
		}


		public bool Equals (BotMessage other)
		{
			// HACK: This is nasty, but I want to be able to compare the message based on timestamp
			//       However, even if I set the timestamp, the server re-sets it so they tend to be
			//       a couple seconds off.  The server also sets the Id, so I don't have that when
			//       originally creating the Activity.
			if (string.IsNullOrEmpty (Activity?.Id) || string.IsNullOrEmpty (other?.Activity?.Id))
			{
				if ((Activity?.LocalTimestamp.HasValue ?? false) && (other?.Activity?.LocalTimestamp.HasValue ?? false))
				{
					return Activity.LocalTimestamp.Value.Equals (Activity.LocalTimestamp.Value);
				}

				return ((Activity?.Text?.Equals (other?.Activity?.Text) ?? false) &&
						(Activity?.Timestamp?.Date.Equals (other?.Activity?.Timestamp?.Date) ?? false) &&
						(Activity?.Timestamp?.Hour.Equals (other?.Activity?.Timestamp?.Hour) ?? false) &&
						(Activity?.Timestamp?.Minute.Equals (other?.Activity?.Timestamp?.Minute) ?? false));
			}

			return Activity.Id.Equals (other.Activity.Id);
		}


		public void Update (Activity activity) => Activity = activity;
	}
}

#endif