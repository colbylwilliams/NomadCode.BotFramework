using System;
namespace NomadCode.BotFramework
{
	public partial class Activity : IActivity, IConversationUpdateActivity, IContactRelationUpdateActivity, IMessageActivity, ITypingActivity, IEndOfConversationActivity, ITriggerActivity
	{
		public ChannelAccount From
		{
			get => FromProperty;
			set => FromProperty = value;
		}

		public static IContactRelationUpdateActivity CreateContactRelationUpdateActivity ()
		{
			throw new NotImplementedException ();
		}

		public static IConversationUpdateActivity CreateConversationUpdateActivity ()
		{
			throw new NotImplementedException ();
		}

		public static IEndOfConversationActivity CreateEndOfConversationActivity ()
		{
			throw new NotImplementedException ();
		}

		public static IMessageActivity CreateMessageActivity ()
		{
			throw new NotImplementedException ();
		}

		public static ITriggerActivity CreateTriggerActivity ()
		{
			throw new NotImplementedException ();
		}

		public static ITypingActivity CreateTypingActivity ()
		{
			throw new NotImplementedException ();
		}

		public static string GetActivityType (string type)
		{
			throw new NotImplementedException ();
		}


		public IContactRelationUpdateActivity AsContactRelationUpdateActivity ()
		{
			throw new NotImplementedException ();
		}

		public IConversationUpdateActivity AsConversationUpdateActivity ()
		{
			throw new NotImplementedException ();
		}

		public IEndOfConversationActivity AsEndOfConversationActivity ()
		{
			throw new NotImplementedException ();
		}

		public IMessageActivity AsMessageActivity ()
		{
			throw new NotImplementedException ();
		}

		public ITriggerActivity AsTriggerActivity ()
		{
			throw new NotImplementedException ();
		}

		public ITypingActivity AsTypingActivity ()
		{
			throw new NotImplementedException ();
		}

		public Activity CreateReply (string text = null, string locale = null)
		{
			throw new NotImplementedException ();
		}

		public string GetActivityType ()
		{
			throw new NotImplementedException ();
		}

		public T GetChannelData<T> ()
		{
			throw new NotImplementedException ();
		}

		public Mention [] GetMentions ()
		{
			throw new NotImplementedException ();
		}

		public bool HasContent ()
		{
			throw new NotImplementedException ();
		}

		protected bool IsActivity (string activity)
		{
			throw new NotImplementedException ();
		}

		public bool MentionsId (string id)
		{
			throw new NotImplementedException ();
		}

		public bool MentionsRecipient ()
		{
			throw new NotImplementedException ();
		}

		public string RemoveMentionText (string id)
		{
			throw new NotImplementedException ();
		}

		public string RemoveRecipientMention ()
		{
			throw new NotImplementedException ();
		}
	}
}
