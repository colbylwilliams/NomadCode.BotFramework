using System.Collections.Generic;

namespace NomadCode.BotFramework
{
	public interface IContactRelationUpdateActivity : IActivity
	{
		IList<ChannelAccount> MembersAdded { get; set; }

		IList<ChannelAccount> MembersRemoved { get; set; }

		string TopicName { get; set; }
	}
}
