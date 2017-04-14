using System;

using UIKit;
using Foundation;

using Xamarin.TTTAttributedLabel;

namespace NomadCode.BotFramework.iOS
{
    public class MessageCell : UITableViewCell, ITTTAttributedLabelDelegate
    {
        public static readonly nfloat AvatarHeight = 24;
        public static readonly nfloat AutoCompleteHeight = 50;


        long loadingTicks;


        #region Views

        UIStackView _buttonStackView;

        TTTAttributedLabel _bodyLabel;

        UILabel _titleLabel, _timestampLabel;

        UIImageView _avatarView, _heroImageView, _thumbnailImageView;

        NSMutableDictionary constraintViews = new NSMutableDictionary ();

        NSMutableDictionary constraintMetrics = NSMutableDictionary.FromObjectsAndKeys (
            new NSNumber [] { NSNumber.FromNFloat (AvatarHeight), NSNumber.FromNFloat (13), NSNumber.FromNFloat (10), NSNumber.FromNFloat (5), NSNumber.FromNFloat (AvatarHeight + 15) },
            new NSString [] { new NSString (@"avatarSize"), new NSString (@"padding"), new NSString (@"right"), new NSString (@"left"), new NSString (@"leftInset") }
        );


        public UIStackView ButtonStackView => _buttonStackView ?? (_buttonStackView = MessageCellSubviews.GetButtonStackView ());

        public TTTAttributedLabel BodyLabel => _bodyLabel ?? (_bodyLabel = MessageCellSubviews.GetBodyLabel (this));

        public UILabel TitleLabel => _titleLabel ?? (_titleLabel = MessageCellSubviews.GetTitleLabel ());

        public UILabel TimestampLabel => _timestampLabel ?? (_timestampLabel = MessageCellSubviews.GetTimestampLabel ());

        public UIImageView AvatarView => _avatarView ?? (_avatarView = MessageCellSubviews.GetAvatarView ());

        public UIImageView HeroImageView => _heroImageView ?? (_heroImageView = MessageCellSubviews.GetHeroImageView ());

        public UIImageView ThumbnailImageView => _thumbnailImageView ?? (_thumbnailImageView = MessageCellSubviews.GetThumbnailImageView ());

        #endregion

        public NSIndexPath IndexPath { get; set; }


        bool? _isHeaderCell;

        public bool IsHeaderCell => _isHeaderCell ?? (_isHeaderCell = ReuseIdentifier.IsHeaderCell ()).Value;

        MessageCellTypes _messageCellType;

        public MessageCellTypes MessageCellType => _messageCellType == MessageCellTypes.Unknown ? (_messageCellType = ReuseIdentifier.MessageCellType ()) : _messageCellType;


        [Export ("initWithStyle:reuseIdentifier:")]
        public MessageCell (UITableViewCellStyle style, NSString reuseIdentifier) : base (style, reuseIdentifier)
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
            BackgroundColor = UIColor.White;

            configureViewsForCellType ();
        }


        public override void PrepareForReuse ()
        {
            base.PrepareForReuse ();

            if (IsHeaderCell)
            {
                TitleLabel.Font = AttributedStringUtilities.HeaderFont;
                TimestampLabel.Font = AttributedStringUtilities.TimestampFont;

                TitleLabel.Text = string.Empty;
                TimestampLabel.Text = string.Empty;
            }

            var views = ButtonStackView.ArrangedSubviews;

            foreach (var view in views)
            {
                ButtonStackView.RemoveArrangedSubview (view);
                view.RemoveFromSuperview ();
                //TODO: Cleanup actions and view
            }

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }


        public void SetMessage (NSAttributedString message, params string [] buttonTitles)
        {
            foreach (var title in buttonTitles)
            {
                ButtonStackView.AddArrangedSubview (MessageCellSubviews.GetButton (title));
            }

            BodyLabel.SetText (message);
        }


        public long SetMessage (DateTime? timestamp, string username, NSAttributedString attrMessage, params string [] buttonTitles)
        {
            loadingTicks = DateTime.UtcNow.Ticks;

            TitleLabel.Text = username;
            TimestampLabel.Text = timestamp?.ToShortTimeString ();

            SetMessage (attrMessage, buttonTitles);

            return loadingTicks;
        }


        public bool SetAvatar (long key, UIImage avatar)
        {
            var same = key == loadingTicks;

            AvatarView.Image = same ? avatar : null;

            return same;
        }


        [Export ("attributedLabel:didSelectLinkWithURL:")]
        public void DidSelectLinkWithURL (TTTAttributedLabel label, NSUrl url)
        {
            Console.WriteLine ($"DidSelectLinkWithURL Label = {label}, Url = {url})");
        }


        void configureViewsForCellType ()
        {
            if (IsHeaderCell)
            {
                configureViewsForHeaderCell ();
            }


            switch (MessageCellType)
            {
                case MessageCellTypes.Message:

                    ContentView.AddSubview (BodyLabel);

                    constraintViews.Add (new NSString (@"contentView"), BodyLabel);

                    break;
                case MessageCellTypes.Hero:

                    break;
                case MessageCellTypes.Thumbnail:

                    break;
                case MessageCellTypes.Receipt:

                    break;
                case MessageCellTypes.Signin:

                    break;
            }


            ContentView.AddSubview (ButtonStackView);

            constraintViews.Add (new NSString (@"stackView"), ButtonStackView);


            configureConstraintsForCellContent ();


            void configureViewsForHeaderCell ()
            {
                ContentView.AddSubview (AvatarView);
                ContentView.AddSubview (TitleLabel);
                ContentView.AddSubview (TimestampLabel);

                constraintViews.Add (new NSString (@"avatarView"), AvatarView);
                constraintViews.Add (new NSString (@"titleLabel"), TitleLabel);
                constraintViews.Add (new NSString (@"timestampLabel"), TimestampLabel);

                configureConstraintsForHeaderCell ();

                void configureConstraintsForHeaderCell ()
                {
                    var hConstraints = @"H:|-left-[avatarView(avatarSize)]-right-[titleLabel(>=0)]-left-[timestampLabel]-(>=right)-|";

                    var vConstraints = @"V:|-padding-[avatarView(avatarSize)]-(>=0)-|";

                    ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (hConstraints, 0, constraintMetrics, constraintViews));

                    ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (vConstraints, 0, constraintMetrics, constraintViews));

                    ContentView.AddConstraint (NSLayoutConstraint.Create (TitleLabel, NSLayoutAttribute.Baseline, NSLayoutRelation.Equal, TimestampLabel, NSLayoutAttribute.Baseline, 1, 0.5f));
                }
            }

            void configureConstraintsForCellContent ()
            {
                var sConstraints = @"H:|-leftInset-[stackView(>=0)]-right-|";

                var hConstraints = @"H:|-leftInset-[contentView(>=0)]-right-|";

                var vConstraints = IsHeaderCell ? @"V:|-right-[titleLabel]-left-[contentView]-left-[stackView]-(>=0@999)-|" : @"V:|[contentView]-left-[stackView]-(>=0@999)-|";

                ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (hConstraints, 0, constraintMetrics, constraintViews));

                ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (vConstraints, 0, constraintMetrics, constraintViews));

                ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (sConstraints, 0, constraintMetrics, constraintViews));
            }
        }
    }
}