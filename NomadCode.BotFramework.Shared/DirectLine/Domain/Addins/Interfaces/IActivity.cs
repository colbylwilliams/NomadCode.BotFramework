using System;

namespace NomadCode.BotFramework
{
	public interface IActivity
	{
		dynamic ChannelData { get; set; }

		string ChannelId { get; set; }

		ConversationAccount Conversation { get; set; }

		ChannelAccount From { get; set; }

		string Id { get; set; }

		DateTimeOffset? LocalTimestamp { get; set; }

		ChannelAccount Recipient { get; set; }

		string ReplyToId { get; set; }

		string ServiceUrl { get; set; }

		DateTime? Timestamp { get; set; }

		string Type { get; set; }


		IContactRelationUpdateActivity AsContactRelationUpdateActivity ();

		IConversationUpdateActivity AsConversationUpdateActivity ();

		IEndOfConversationActivity AsEndOfConversationActivity ();

		IMessageActivity AsMessageActivity ();

		ITriggerActivity AsTriggerActivity ();

		ITypingActivity AsTypingActivity ();
	}
}
