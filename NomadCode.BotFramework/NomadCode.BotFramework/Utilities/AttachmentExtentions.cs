using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using Microsoft.Bot.Connector.DirectLine;

namespace NomadCode.BotFramework
{
    public static class CardContentTypes
    {
        public const string Animation = @"application/vnd.microsoft.card.animation";
        public const string Audio = @"application/vnd.microsoft.card.audio";
        public const string Hero = @"application/vnd.microsoft.card.hero";
        public const string Thumbnail = @"application/vnd.microsoft.card.thumbnail";
        public const string Receipt = @"application/vnd.microsoft.card.receipt";
        public const string Signin = @"application/vnd.microsoft.card.signin";
        public const string Video = @"application/vnd.microsoft.card.video";
    }


    public enum CardTypes
    {
        Unknown,
        Animation,
        Audio,
        Hero,
        Thumbnail,
        Receipt,
        Signin,
        Video
    }


    public static class AttachmentExtentions
    {

        public static AttachmentContent GetContent (this Attachment attachment)
        {
            if (attachment.Content is JObject content)
            {
                var card = content.ToObject<AttachmentContent> ();

                if (card != null)
                {
                    return card;
                }
            }

            return new AttachmentContent ();
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


        public static HeroCard AsHeroCard (this Attachment attachment) => attachment.asCard<HeroCard> (CardContentTypes.Hero);

        public static SigninCard AsSigninCard (this Attachment attachment) => attachment.asCard<SigninCard> (CardContentTypes.Signin);

        public static ReceiptCard AsReceiptCard (this Attachment attachment) => attachment.asCard<ReceiptCard> (CardContentTypes.Receipt);

        public static ThumbnailCard AsThumbnailCard (this Attachment attachment) => attachment.asCard<ThumbnailCard> (CardContentTypes.Thumbnail);


        public static List<CardAction> GetButtons (this Attachment attachment)
        {
            switch (attachment.ContentType)
            {
                case CardContentTypes.Hero: return attachment.AsHeroCard ()?.Buttons.ToList () ?? new List<CardAction> ();
                case CardContentTypes.Thumbnail: return attachment.AsThumbnailCard ()?.Buttons.ToList () ?? new List<CardAction> ();
                case CardContentTypes.Receipt: return attachment.AsReceiptCard ()?.Buttons.ToList () ?? new List<CardAction> ();
                case CardContentTypes.Signin: return attachment.AsSigninCard ()?.Buttons.ToList () ?? new List<CardAction> ();
                default: return new List<CardAction> ();
                    //case CardContentTypes.Audio:
                    //case CardContentTypes.Animation:
                    //case CardContentTypes.Video:
            }
        }


        //public static int GetButtonCount (this Attachment attachment)
        //{
        //    if (attachment.IsCard () && attachment.Content is JObject content)
        //    {
        //        return content.GetValue ("buttons").Values<CardAction> ().Count ();
        //    }
        //    return 0;
        //}


        static T asCard<T> (this Attachment attachment, string contentType)
            where T : class
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
