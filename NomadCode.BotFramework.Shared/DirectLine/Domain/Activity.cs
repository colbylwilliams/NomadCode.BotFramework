
namespace NomadCode.BotFramework
{
	using Newtonsoft.Json;
	using NomadCode.BotFramework;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// An Activity is the basic communication type for the Bot Framework 3.0
	/// protocol
	/// </summary>
	public partial class Activity
	{
		/// <summary>
		/// Initializes a new instance of the Activity class.
		/// </summary>
		public Activity ()
		{
			CustomInit ();
		}

		/// <summary>
		/// Initializes a new instance of the Activity class.
		/// </summary>
		/// <param name="type">The type of the activity
		/// [message|contactRelationUpdate|converationUpdate|typing]</param>
		/// <param name="id">Id for the activity</param>
		/// <param name="timestamp">UTC Time when message was sent (Set by
		/// service)</param>
		/// <param name="localTimestamp">Local time when message was sent (set
		/// by client Ex: 2016-09-23T13:07:49.4714686-07:00)</param>
		/// <param name="serviceUrl">Service endpoint</param>
		/// <param name="channelId">ChannelId the activity was on</param>
		/// <param name="fromProperty">Sender address</param>
		/// <param name="conversation">Conversation</param>
		/// <param name="recipient">(Outbound to bot only) Bot's address that
		/// received the message</param>
		/// <param name="textFormat">Format of text fields [plain|markdown]
		/// Default:markdown</param>
		/// <param name="attachmentLayout">AttachmentLayout - hint for how to
		/// deal with multiple attachments Values: [list|carousel]
		/// Default:list</param>
		/// <param name="membersAdded">Array of address added</param>
		/// <param name="membersRemoved">Array of addresses removed</param>
		/// <param name="topicName">Conversations new topic name</param>
		/// <param name="historyDisclosed">the previous history of the channel
		/// was disclosed</param>
		/// <param name="locale">The language code of the Text field</param>
		/// <param name="text">Content for the message</param>
		/// <param name="summary">Text to display if you can't render
		/// cards</param>
		/// <param name="attachments">Attachments</param>
		/// <param name="entities">Collection of Entity objects, each of which
		/// contains metadata about this activity. Each Entity object is
		/// typed.</param>
		/// <param name="channelData">Channel specific payload</param>
		/// <param name="action">ContactAdded/Removed action</param>
		/// <param name="replyToId">the original id this message is a response
		/// to</param>
		/// <param name="value">Open ended value</param>
		public Activity (string type = default (string), string id = default (string), System.DateTime? timestamp = default (System.DateTime?), System.DateTime? localTimestamp = default (System.DateTime?), string serviceUrl = default (string), string channelId = default (string), ChannelAccount fromProperty = default (ChannelAccount), ConversationAccount conversation = default (ConversationAccount), ChannelAccount recipient = default (ChannelAccount), string textFormat = default (string), string attachmentLayout = default (string), IList<ChannelAccount> membersAdded = default (IList<ChannelAccount>), IList<ChannelAccount> membersRemoved = default (IList<ChannelAccount>), string topicName = default (string), bool? historyDisclosed = default (bool?), string locale = default (string), string text = default (string), string summary = default (string), IList<Attachment> attachments = default (IList<Attachment>), IList<Entity> entities = default (IList<Entity>), object channelData = default (object), string action = default (string), string replyToId = default (string), object value = default (object))
		{
			Type = type;
			Id = id;
			Timestamp = timestamp;
			LocalTimestamp = localTimestamp;
			ServiceUrl = serviceUrl;
			ChannelId = channelId;
			FromProperty = fromProperty;
			Conversation = conversation;
			Recipient = recipient;
			TextFormat = textFormat;
			AttachmentLayout = attachmentLayout;
			MembersAdded = membersAdded;
			MembersRemoved = membersRemoved;
			TopicName = topicName;
			HistoryDisclosed = historyDisclosed;
			Locale = locale;
			Text = text;
			Summary = summary;
			Attachments = attachments;
			Entities = entities;
			ChannelData = channelData;
			Action = action;
			ReplyToId = replyToId;
			Value = value;
			CustomInit ();
		}

		/// <summary>
		/// An initialization method that performs custom operations like setting defaults
		/// </summary>
		partial void CustomInit ();

		/// <summary>
		/// Gets or sets the type of the activity
		/// [message|contactRelationUpdate|converationUpdate|typing]
		/// </summary>
		[JsonProperty (PropertyName = "type")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets id for the activity
		/// </summary>
		[JsonProperty (PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets UTC Time when message was sent (Set by service)
		/// </summary>
		[JsonProperty (PropertyName = "timestamp")]
		public System.DateTime? Timestamp { get; set; }

		/// <summary>
		/// Gets or sets local time when message was sent (set by client Ex:
		/// 2016-09-23T13:07:49.4714686-07:00)
		/// </summary>
		[JsonProperty (PropertyName = "localTimestamp")]
		public System.DateTimeOffset? LocalTimestamp { get; set; }

		/// <summary>
		/// Gets or sets service endpoint
		/// </summary>
		[JsonProperty (PropertyName = "serviceUrl")]
		public string ServiceUrl { get; set; }

		/// <summary>
		/// Gets or sets channelId the activity was on
		/// </summary>
		[JsonProperty (PropertyName = "channelId")]
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets sender address
		/// </summary>
		[JsonProperty (PropertyName = "from")]
		public ChannelAccount FromProperty { get; set; }

		/// <summary>
		/// Gets or sets conversation
		/// </summary>
		[JsonProperty (PropertyName = "conversation")]
		public ConversationAccount Conversation { get; set; }

		/// <summary>
		/// Gets or sets (Outbound to bot only) Bot's address that received the
		/// message
		/// </summary>
		[JsonProperty (PropertyName = "recipient")]
		public ChannelAccount Recipient { get; set; }

		/// <summary>
		/// Gets or sets format of text fields [plain|markdown]
		/// Default:markdown
		/// </summary>
		[JsonProperty (PropertyName = "textFormat")]
		public string TextFormat { get; set; }

		/// <summary>
		/// Gets or sets attachmentLayout - hint for how to deal with multiple
		/// attachments Values: [list|carousel] Default:list
		/// </summary>
		[JsonProperty (PropertyName = "attachmentLayout")]
		public string AttachmentLayout { get; set; }

		/// <summary>
		/// Gets or sets array of address added
		/// </summary>
		[JsonProperty (PropertyName = "membersAdded")]
		public IList<ChannelAccount> MembersAdded { get; set; }

		/// <summary>
		/// Gets or sets array of addresses removed
		/// </summary>
		[JsonProperty (PropertyName = "membersRemoved")]
		public IList<ChannelAccount> MembersRemoved { get; set; }

		/// <summary>
		/// Gets or sets conversations new topic name
		/// </summary>
		[JsonProperty (PropertyName = "topicName")]
		public string TopicName { get; set; }

		/// <summary>
		/// Gets or sets the previous history of the channel was disclosed
		/// </summary>
		[JsonProperty (PropertyName = "historyDisclosed")]
		public bool? HistoryDisclosed { get; set; }

		/// <summary>
		/// Gets or sets the language code of the Text field
		/// </summary>
		[JsonProperty (PropertyName = "locale")]
		public string Locale { get; set; }

		/// <summary>
		/// Gets or sets content for the message
		/// </summary>
		[JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets text to display if you can't render cards
		/// </summary>
		[JsonProperty (PropertyName = "summary")]
		public string Summary { get; set; }

		/// <summary>
		/// Gets or sets attachments
		/// </summary>
		[JsonProperty (PropertyName = "attachments")]
		public IList<Attachment> Attachments { get; set; }

		/// <summary>
		/// Gets or sets collection of Entity objects, each of which contains
		/// metadata about this activity. Each Entity object is typed.
		/// </summary>
		[JsonProperty (PropertyName = "entities")]
		public IList<Entity> Entities { get; set; }

		/// <summary>
		/// Gets or sets channel specific payload
		/// </summary>
		[JsonProperty (PropertyName = "channelData")]
		public object ChannelData { get; set; }

		/// <summary>
		/// Gets or sets contactAdded/Removed action
		/// </summary>
		[JsonProperty (PropertyName = "action")]
		public string Action { get; set; }

		/// <summary>
		/// Gets or sets the original id this message is a response to
		/// </summary>
		[JsonProperty (PropertyName = "replyToId")]
		public string ReplyToId { get; set; }

		/// <summary>
		/// Gets or sets open ended value
		/// </summary>
		[JsonProperty (PropertyName = "value")]
		public object Value { get; set; }

	}
}
