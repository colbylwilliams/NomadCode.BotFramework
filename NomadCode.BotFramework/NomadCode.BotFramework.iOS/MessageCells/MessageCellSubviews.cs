using System;

using Foundation;
using UIKit;

using Xamarin.TTTAttributedLabel;

namespace NomadCode.BotFramework.iOS
{
    public static class MessageCellSubviews
    {
        public static TTTAttributedLabel GetBodyLabel (NSObject weakDelegate)
        {
            var bodyLabel = new TTTAttributedLabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                UserInteractionEnabled = true,
                Lines = 0,
                TextColor = Colors.MessageColor,
                Font = AttributedStringUtilities.MessageFont,
                LinkAttributes = AttributedStringUtilities.LinkStringAttributes.Dictionary,
                EnabledTextCheckingTypes = NSTextCheckingType.Link,
                WeakDelegate = weakDelegate
            };

            bodyLabel.SetContentCompressionResistancePriority (250, UILayoutConstraintAxis.Vertical);

            return bodyLabel;
        }


        public static UILabel GetTitleLabel ()
        {
            var titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                UserInteractionEnabled = false,
                Lines = 0,
                TextColor = Colors.MessageColor,
                Font = AttributedStringUtilities.HeaderFont,
            };

            titleLabel.SetContentCompressionResistancePriority (300, UILayoutConstraintAxis.Vertical);

            return titleLabel;
        }


        public static TTTAttributedLabel GetAttachmentTitleLabel (NSObject weakDelegate)
        {
            var titleLabel = new TTTAttributedLabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                UserInteractionEnabled = true,
                Lines = 0,
                TextColor = Colors.MessageColor,
                Font = AttributedStringUtilities.AttachmentTitleFont,
                LinkAttributes = AttributedStringUtilities.LinkStringAttributes.Dictionary,
                EnabledTextCheckingTypes = NSTextCheckingType.Link,
                WeakDelegate = weakDelegate
            };

            titleLabel.SetContentCompressionResistancePriority (250, UILayoutConstraintAxis.Vertical);

            return titleLabel;
        }


        public static TTTAttributedLabel GetAttachmentSubtitleLabel (NSObject weakDelegate)
        {
            var subtitleLabel = new TTTAttributedLabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                UserInteractionEnabled = true,
                Lines = 0,
                TextColor = UIColor.DarkGray,
                Font = AttributedStringUtilities.AttachmentSubtitleFont,
                LinkAttributes = AttributedStringUtilities.LinkStringAttributes.Dictionary,
                EnabledTextCheckingTypes = NSTextCheckingType.Link,
                WeakDelegate = weakDelegate
            };

            subtitleLabel.SetContentCompressionResistancePriority (250, UILayoutConstraintAxis.Vertical);

            return subtitleLabel;
        }


        public static UILabel GetTimestampLabel ()
        {
            var timestampLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.White,
                UserInteractionEnabled = false,
                Lines = 0,
                TextColor = UIColor.LightGray,
                Font = AttributedStringUtilities.TimestampFont
            };

            return timestampLabel;
        }


        public static UIImageView GetAvatarView ()
        {
            var avatarView = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                UserInteractionEnabled = false,
                BackgroundColor = UIColor.FromWhiteAlpha (0.9f, 1.0f),
            };

            avatarView.Layer.CornerRadius = 4;
            avatarView.Layer.MasksToBounds = true;

            return avatarView;
        }


        public static UIImageView GetHeroImageView ()
        {
            var heroImageView = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                UserInteractionEnabled = false,
                ContentMode = UIViewContentMode.ScaleAspectFit,
                BackgroundColor = UIColor.FromWhiteAlpha (0.9f, 1.0f)
            };

            heroImageView.Layer.CornerRadius = 4;
            heroImageView.Layer.MasksToBounds = true;

            heroImageView.SetContentCompressionResistancePriority (200, UILayoutConstraintAxis.Vertical);

            return heroImageView;
        }


        public static UIImageView GetThumbnailImageView ()
        {
            var thumbnailImageView = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                UserInteractionEnabled = false,
                BackgroundColor = UIColor.FromWhiteAlpha (0.9f, 1.0f),
            };

            thumbnailImageView.Layer.CornerRadius = 4;
            thumbnailImageView.Layer.MasksToBounds = true;

            return thumbnailImageView;
        }


        public static UIButton GetButton (string title)
        {
            var button = new UIButton (UIButtonType.System)
            {
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            button.SetTitle (title, UIControlState.Normal);
            button.SetTitleColor (UIColor.DarkGray, UIControlState.Normal);

            button.TitleLabel.Font = UIFont.BoldSystemFontOfSize (AttributedStringUtilities.MessageFontSize);

            button.Layer.BorderWidth = 1;
            button.Layer.BorderColor = UIColor.LightGray.CGColor;
            button.Layer.CornerRadius = 4;

            return button;
        }


        public static UIStackView GetContentStackView ()
        {
            var stackView = new UIStackView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Axis = UILayoutConstraintAxis.Vertical,
                Spacing = 4,
                Alignment = UIStackViewAlignment.Fill,
                Distribution = UIStackViewDistribution.Fill
            };

            stackView.SetContentCompressionResistancePriority (200, UILayoutConstraintAxis.Vertical);

            return stackView;
        }
    }
}
