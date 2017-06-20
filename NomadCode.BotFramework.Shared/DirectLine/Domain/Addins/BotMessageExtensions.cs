using System.Collections.Generic;

namespace NomadCode.BotFramework
{
	public static class BotMessageExtensions
	{
		public static bool IsHead (this List<BotMessage> messages, int index)
		{
			return index == messages.Count - 1 || (index + 1 < messages.Count) && (messages [index + 1].Activity.From.Name != messages [index].Activity.From.Name);
		}
	}
}
