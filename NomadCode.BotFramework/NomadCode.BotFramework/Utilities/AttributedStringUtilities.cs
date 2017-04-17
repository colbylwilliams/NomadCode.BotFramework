#if __IOS__

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UIKit;

using Foundation;

namespace NomadCode.BotFramework
{
    public enum MarkdownFmtStringType
    {
        Unknown,
        Link,
        Bold,
        Italic,
        Strikethrough,
        UnorderedList,
        OrderedList,
        Pre,
        BlockQuote,
        Image
    }


    public class MarkdownFmtString
    {
        public MarkdownFmtStringType FmtStringType { get; set; }

        public string Original { get; set; }

        public string Display { get; set; }

        public NSUrl Link { get; set; }
    }


    public static class AttributedStringUtilities
    {
        const string linkFmt = @"(?<!\!)\[(.*?)\)";
        const string boldFmt = @"\*\*(.*?)\*\*";
        const string italicFmt = @"(?<!\*)\*{1}([^*].*?)\*{1}(?!\*)";
        const string imageFmt = @"\!\[(.*?)\)";
        const string strikeFmt = @"\~\~(.*?)\~\~";
        const string ulFmt = @"(?<=\n)[*•-]\s+(?!\s)";
        const string olFmt = @"(?<=\n)\d+\.\s+(?!\s)";
        const string preFmt = @"\`(.*?)\`";
        const string quoteFmt = @"\s+\>+\s+(.*)";


        static readonly char [] linkTrim = { '[', ')' };
        static readonly char [] boldTrim = { '*' };
        static readonly char [] italicTrim = { '*' };
        static readonly char [] imageTrim = { '!', '[', ')' };
        static readonly char [] strikeTrim = { '~' };
        static readonly char [] preTrim = { '`' };

        static readonly (char Item, char With) ulReplace = ('*', '•');


        static readonly string [] linkSplit = { "](" };


        public static nfloat HeaderFontSize = 18;
        public static nfloat MessageFontSize = 16;
        public static nfloat TimestampFontSize = 11;

        public static readonly UIFont HeaderFont = UIFont.BoldSystemFontOfSize (HeaderFontSize);
        public static readonly UIFont MessageFont = UIFont.SystemFontOfSize (MessageFontSize);
        public static readonly UIFont TimestampFont = UIFont.SystemFontOfSize (TimestampFontSize);
        public static readonly UIFont AttachmentTitleFont = UIFont.BoldSystemFontOfSize (MessageFontSize);
        public static readonly UIFont AttachmentSubtitleFont = UIFont.SystemFontOfSize (MessageFontSize - 1);

        static UIFont messageFontBold = UIFont.BoldSystemFontOfSize (MessageFontSize);
        static UIFont messageFontItalic = UIFont.ItalicSystemFontOfSize (MessageFontSize);
        static UIFont messageFontPre = UIFont.FromName ("Menlo-Regular", MessageFontSize - 1);


        static readonly NSMutableParagraphStyle messageParagraphStyle = new NSMutableParagraphStyle
        {
            LineBreakMode = UILineBreakMode.WordWrap,
            Alignment = UITextAlignment.Left
        };

        static readonly NSMutableParagraphStyle listItemParagraphStyle = new NSMutableParagraphStyle
        {
            LineBreakMode = UILineBreakMode.WordWrap,
            Alignment = UITextAlignment.Left,
            FirstLineHeadIndent = 0,
            HeadIndent = 15
        };


        public static NSMutableAttributedString GetAttributedString (this string text, UIStringAttributes attributes)
        {
            var attrString = new NSMutableAttributedString (text);

            attrString.AddAttributes (attributes, new NSRange (0, text.Length));

            return attrString;
        }


        public static NSMutableAttributedString GetAttachmentTitleAttributedString (this string title)
        {
            return GetAttributedString (title, AttachmentTitleStringAttributes);
        }

        public static NSMutableAttributedString GetAttachmentSubtitleAttributedString (this string title)
        {
            return GetAttributedString (title, AttachmentSubtitleStringAttributes);
        }


        public static NSMutableAttributedString GetMessageAttributedString (this string message)
        {
            var markdownFmtStrings = new List<MarkdownFmtString> ();

            //message = message.Replace ("\r\n*", "\r\n•");

            //Log.Debug (message);

            var messageCopy = message;

            for (Match match = Regex.Match (message, linkFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Link));

            for (Match match = Regex.Match (message, imageFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Image));

            for (Match match = Regex.Match (message, boldFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Bold));

            for (Match match = Regex.Match (message, italicFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Italic));

            for (Match match = Regex.Match (message, ulFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.UnorderedList));

            for (Match match = Regex.Match (message, olFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.OrderedList));

            for (Match match = Regex.Match (message, strikeFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Strikethrough));

            for (Match match = Regex.Match (message, preFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.Pre));

            for (Match match = Regex.Match (message, quoteFmt); match.Success; match = match.NextMatch ())
                markdownFmtStrings.Add (match.Value.ToMarkdownFmtString (MarkdownFmtStringType.BlockQuote));


            foreach (var markdownString in markdownFmtStrings)
            {
                messageCopy = messageCopy.Replace (markdownString.Original, markdownString.Display);
            }


            var attrString = new NSMutableAttributedString (messageCopy);

            attrString.AddAttributes (MessageStringAttributes, new NSRange (0, messageCopy.Length));


            foreach (var markdownString in markdownFmtStrings)
            {
                var range = new NSRange (messageCopy.IndexOf (markdownString.Display, StringComparison.Ordinal), markdownString.Display.Length);

                switch (markdownString.FmtStringType)
                {
                    case MarkdownFmtStringType.Unknown:

                        break;
                    case MarkdownFmtStringType.Link:

                        if (markdownString.Link != null)
                            attrString.AddAttribute (UIStringAttributeKey.Link, markdownString.Link, range);

                        attrString.AddAttributes (LinkStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.Bold:

                        attrString.AddAttributes (BoldStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.Italic:

                        attrString.AddAttributes (ItalicStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.Strikethrough:

                        attrString.AddAttributes (StrikeStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.UnorderedList:

                        //attrString.AddAttributes(ListItemStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.OrderedList:

                        //attrString.AddAttributes (LinkStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.Pre:

                        attrString.AddAttributes (PreStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.BlockQuote:

                        //attrString.AddAttributes (MessageStringAttributes, range);

                        break;
                    case MarkdownFmtStringType.Image:

                        if (markdownString.Link != null)
                            attrString.AddAttribute (UIStringAttributeKey.Link, markdownString.Link, range);

                        attrString.AddAttributes (LinkStringAttributes, range);

                        break;
                }
            }

            return attrString;
        }


        public static MarkdownFmtString ToMarkdownFmtString (this string orig, MarkdownFmtStringType fmtStringType)
        {
            var markdownString = new MarkdownFmtString
            {
                Original = orig,
                FmtStringType = fmtStringType
            };

            switch (fmtStringType)
            {
                case MarkdownFmtStringType.Unknown:

                    break;
                case MarkdownFmtStringType.Link:

                    var linkStringArr = orig.Trim (linkTrim).Split (linkSplit, StringSplitOptions.RemoveEmptyEntries);

                    markdownString.Display = linkStringArr [0];

                    markdownString.Link = (linkStringArr.Length > 1) ? NSUrl.FromString (linkStringArr [1]) : null;

                    break;
                case MarkdownFmtStringType.Bold:

                    markdownString.Display = orig.Trim (boldTrim);

                    break;
                case MarkdownFmtStringType.Italic:

                    markdownString.Display = orig.Trim (italicTrim);

                    break;
                case MarkdownFmtStringType.Strikethrough:

                    markdownString.Display = orig.Trim (strikeTrim);

                    break;
                case MarkdownFmtStringType.UnorderedList:

                    markdownString.Display = orig.Replace (ulReplace.Item, ulReplace.With);

                    break;
                case MarkdownFmtStringType.OrderedList:

                    markdownString.Display = orig;

                    break;
                case MarkdownFmtStringType.Pre:

                    markdownString.Display = orig.Trim (preTrim);

                    break;
                case MarkdownFmtStringType.BlockQuote:

                    markdownString.Display = orig;

                    break;
                case MarkdownFmtStringType.Image:

                    var imageStringArr = orig.Trim (imageTrim).Split (linkSplit, StringSplitOptions.RemoveEmptyEntries);

                    markdownString.Display = imageStringArr [0];

                    markdownString.Link = (imageStringArr.Length > 1) ? NSUrl.FromString (imageStringArr [1]) : null;

                    break;
            }

            return markdownString;
        }


        #region UIStringAttributes

        public static UIStringAttributes AttachmentTitleStringAttributes = new UIStringAttributes
        {
            Font = AttachmentTitleFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes AttachmentSubtitleStringAttributes = new UIStringAttributes
        {
            Font = AttachmentSubtitleFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = UIColor.DarkGray
        };


        public static UIStringAttributes MessageStringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes ListItemStringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = listItemParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes LinkStringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            UnderlineStyle = NSUnderlineStyle.None,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageLinkColor
        };

        public static UIStringAttributes BoldStringAttributes = new UIStringAttributes
        {
            Font = messageFontBold,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes ItalicStringAttributes = new UIStringAttributes
        {
            Font = messageFontItalic,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes StrikeStringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            StrikethroughStyle = NSUnderlineStyle.Single,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes PreStringAttributes = new UIStringAttributes
        {
            Font = messageFontPre,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor,
            BackgroundColor = Colors.MessagePreColor
        };

        public static UIStringAttributes H1StringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes H2StringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes H3StringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes H4StringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        public static UIStringAttributes H5StringAttributes = new UIStringAttributes
        {
            Font = MessageFont,
            ParagraphStyle = messageParagraphStyle,
            ForegroundColor = Colors.MessageColor
        };

        #endregion

    }
}

#endif