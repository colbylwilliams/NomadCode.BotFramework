using System.Collections.Generic;

namespace NomadCode.BotFramework
{
	public interface ICard
	{
		string Title { get; set; }

		string Text { get; set; }

		IList<CardAction> Buttons { get; set; }
	}
}
