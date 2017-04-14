using Foundation;

namespace NomadCode.BotFramework.iOS
{
    public enum MessageCellTypes
    {
        Unknown,
        Message,
        Hero,
        Thumbnail,
        Receipt,
        Signin
    }

    public static class MessageCellReuseIds
    {
        public static readonly NSString MessageCellReuseId = new NSString ("MessageCellReuseId");
        public static readonly NSString MessageHeaderCellReuseId = new NSString ("MessageHeaderCellReuseId");

        public static readonly NSString HeroCellReuseId = new NSString ("HeroCellReuseId");
        public static readonly NSString HeroHeaderCellReuseId = new NSString ("HeroHeaderCellReuseId");

        public static readonly NSString ThumbnailCellReuseId = new NSString ("ThumbnailCellReuseId");
        public static readonly NSString ThumbnailHeaderCellReuseId = new NSString ("ThumbnailHeaderCellReuseId");

        public static readonly NSString ReceiptCellReuseId = new NSString ("ReceiptCellReuseId");
        public static readonly NSString ReceiptHeaderCellReuseId = new NSString ("ReceiptHeaderCellReuseId");

        public static readonly NSString SigninCellReuseId = new NSString ("SigninCellReuseId");
        public static readonly NSString SigninHeaderCellReuseId = new NSString ("SigninHeaderCellReuseId");

        public static readonly NSString AutoCompleteReuseId = new NSString ("AutoCompletionCell");
    }

    public static class MessageCellTypeExtensions
    {
        public static MessageCellTypes MessageCellType (this NSString reuseIdentifier)
        {
            if (reuseIdentifier == MessageCellReuseIds.MessageCellReuseId || reuseIdentifier == MessageCellReuseIds.MessageHeaderCellReuseId)
            {
                return MessageCellTypes.Message;
            }

            if (reuseIdentifier == MessageCellReuseIds.HeroCellReuseId || reuseIdentifier == MessageCellReuseIds.HeroHeaderCellReuseId)
            {
                return MessageCellTypes.Hero;
            }

            if (reuseIdentifier == MessageCellReuseIds.ThumbnailCellReuseId || reuseIdentifier == MessageCellReuseIds.ThumbnailHeaderCellReuseId)
            {
                return MessageCellTypes.Thumbnail;
            }

            if (reuseIdentifier == MessageCellReuseIds.ReceiptCellReuseId || reuseIdentifier == MessageCellReuseIds.ReceiptHeaderCellReuseId)
            {
                return MessageCellTypes.Receipt;
            }

            if (reuseIdentifier == MessageCellReuseIds.SigninCellReuseId || reuseIdentifier == MessageCellReuseIds.SigninHeaderCellReuseId)
            {
                return MessageCellTypes.Signin;
            }

            return MessageCellTypes.Unknown;
        }

        public static bool IsHeaderCell (this NSString reuseIdentifier)
        {
            return reuseIdentifier == MessageCellReuseIds.MessageHeaderCellReuseId
                || reuseIdentifier == MessageCellReuseIds.HeroHeaderCellReuseId
                || reuseIdentifier == MessageCellReuseIds.ThumbnailHeaderCellReuseId
                || reuseIdentifier == MessageCellReuseIds.ReceiptHeaderCellReuseId
                || reuseIdentifier == MessageCellReuseIds.SigninHeaderCellReuseId;
        }
    }
}
