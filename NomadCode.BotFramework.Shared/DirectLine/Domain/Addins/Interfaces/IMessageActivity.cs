using System.Collections.Generic;

namespace NomadCode.BotFramework
{
	public interface IMessageActivity : IActivity
	{
		string AttachmentLayout { get; set; }

		IList<Attachment> Attachments { get; set; }

		IList<Entity> Entities { get; set; }

		string Locale { get; set; }

		string Summary { get; set; }

		string Text { get; set; }

		string TextFormat { get; set; }


		T GetChannelData<T> ();

		Mention [] GetMentions ();

		bool HasContent ();
	}
}
