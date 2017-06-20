namespace NomadCode.BotFramework
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class LitwareChannelData
	{
		[JsonProperty (PropertyName = "Username")]
		public string Username { get; set; }

		[JsonProperty (PropertyName = "AccountId")]
		public string AccountId { get; set; }

		//[JsonProperty (PropertyName = "Attachments")]
		//public LitwareChannelDataAttachment Attachments { get; set; }
	}

	public class LitwareChannelDataAttachment
	{
		[JsonProperty (PropertyName = "contentType")]
		public string ContentType { get; set; }

		[JsonProperty (PropertyName = "content")]
		public LitwareChannelDataAttachmentContent Content { get; set; }
	}

	public class LitwareChannelDataAttachmentContent
	{
		[JsonProperty (PropertyName = "Murphy")]
		public string Murphy { get; set; }

		[JsonProperty (PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty (PropertyName = "images")]
		public IList<CardImage> Images { get; set; }

		[JsonProperty (PropertyName = "buttons")]
		public IList<CardAction> Buttons { get; set; }

		[JsonProperty (PropertyName = "IsHuman")]
		public bool IsHuman { get; set; }

		[JsonProperty (PropertyName = "IsJoining")]
		public bool IsJoining { get; set; }

	}
}
