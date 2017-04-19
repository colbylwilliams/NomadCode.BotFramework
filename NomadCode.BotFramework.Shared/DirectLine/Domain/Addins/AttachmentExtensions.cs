using System;
using Newtonsoft.Json.Linq;

namespace NomadCode.BotFramework
{
	public static class AttachmentExtensions
	{
		public static CardContent GetContent (this Attachment attachment)
		{
			if (attachment.Content is JObject content)
			{
				var card = content.ToObject<CardContent> ();

				if (card != null)
				{
					return card;
				}
			}

			return new CardContent ();
		}

		public static bool IsCard (this Attachment attachment) => attachment.GetCardType () != CardTypes.Unknown;

		public static CardTypes GetCardType (this Attachment attachment)
		{
			switch (attachment.ContentType)
			{
				case CardContentTypes.Animation: return CardTypes.Animation;
				case CardContentTypes.Audio: return CardTypes.Audio;
				case CardContentTypes.Hero: return CardTypes.Hero;
				case CardContentTypes.Thumbnail: return CardTypes.Thumbnail;
				case CardContentTypes.Receipt: return CardTypes.Receipt;
				case CardContentTypes.Signin: return CardTypes.Signin;
				case CardContentTypes.Video: return CardTypes.Video;
				default: return CardTypes.Unknown;
			}
		}


		public static AnimationCard AsAnimationCard (this Attachment attachment) => attachment.asCard<AnimationCard> (AnimationCard.ContentType);

		public static AudioCard AsAudioCard (this Attachment attachment) => attachment.asCard<AudioCard> (AudioCard.ContentType);

		public static HeroCard AsHeroCard (this Attachment attachment) => attachment.asCard<HeroCard> (HeroCard.ContentType);

		public static ThumbnailCard AsThumbnailCard (this Attachment attachment) => attachment.asCard<ThumbnailCard> (ThumbnailCard.ContentType);

		public static ReceiptCard AsReceiptCard (this Attachment attachment) => attachment.asCard<ReceiptCard> (ReceiptCard.ContentType);

		public static SigninCard AsSigninCard (this Attachment attachment) => attachment.asCard<SigninCard> (SigninCard.ContentType);

		public static VideoCard AsVideoCard (this Attachment attachment) => attachment.asCard<VideoCard> (VideoCard.ContentType);

		static T asCard<T> (this Attachment attachment, string contentType)
			where T : class, ICard
		{
			if (attachment.ContentType != contentType)
			{
				throw new InvalidOperationException ($"The attachment is not a {typeof (T).Name}.  This extension method can only be used on attachments with contentType == {contentType}. This attachment has contentType == {attachment?.ContentType}.");
			}

			if (attachment.Content is JObject content)
			{
				var card = content.ToObject<T> ();

				if (card != null)
				{
					return card;
				}
			}

			return default (T);
		}
	}
}
