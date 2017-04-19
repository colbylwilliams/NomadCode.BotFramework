using Newtonsoft.Json;

namespace NomadCode.BotFramework
{
	public partial class ConversationReference
	{
		[JsonProperty (PropertyName = "activityId")]
		public string ActivityId { get; set; }


		[JsonProperty (PropertyName = "bot")]
		public ChannelAccount Bot { get; set; }


		[JsonProperty (PropertyName = "channelId")]
		public string ChannelId { get; set; }


		[JsonProperty (PropertyName = "conversation")]
		public ConversationAccount Conversation { get; set; }


		[JsonProperty (PropertyName = "serviceUrl")]
		public string ServiceUrl { get; set; }


		[JsonProperty (PropertyName = "user")]
		public ChannelAccount User { get; set; }


		/// <summary>
		/// An initialization method that performs custom operations like setting defaults
		/// </summary>
		partial void CustomInit ();

		public ConversationReference ()
		{
			CustomInit ();
		}

		public ConversationReference (string activityId = null, ChannelAccount user = null, ChannelAccount bot = null, ConversationAccount conversation = null, string channelId = null, string serviceUrl = null)
		{
			ActivityId = activityId;
			User = user;
			Bot = bot;
			Conversation = conversation;
			ChannelId = channelId;
			ServiceUrl = serviceUrl;
			CustomInit ();
		}
	}
}
