using System;
namespace NomadCode.BotFramework
{
	public partial class ReceiptCard : ICard
	{
		public const string ContentType = @"application/vnd.microsoft.com.card.receipt";

		public string Text
		{
			get => Title;
			set => Title = value;
		}
	}
}
