namespace NomadCode.BotFramework
{
	public partial class Attachment
	{
		public bool IsCard => CardType != CardTypes.Unknown;



		CardTypes? _cardType;

		public CardTypes CardType
		{
			get
			{
				if (!_cardType.HasValue)
				{
					_cardType = getCardType ();
				}
				return _cardType.Value;
			}
		}


		CardTypes getCardType ()
		{
			switch (ContentType)
			{
				case AnimationCard.ContentType: return CardTypes.Animation;
				case AudioCard.ContentType: return CardTypes.Audio;
				case HeroCard.ContentType: return CardTypes.Hero;
				case ThumbnailCard.ContentType: return CardTypes.Thumbnail;
				case ReceiptCard.ContentType: return CardTypes.Receipt;
				case SigninCard.ContentType: return CardTypes.Signin;
				case VideoCard.ContentType: return CardTypes.Video;
				default: return CardTypes.Unknown;
			}
		}
	}
}