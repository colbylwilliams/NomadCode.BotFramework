using System;
namespace NomadCode.BotFramework
{
	public partial class SigninCard : ICard
	{
		public const string ContentType = @"application/vnd.microsoft.com.card.signin";

		public string Title
		{
			get => Text;
			set => Text = value;
		}
	}
}
