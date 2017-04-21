using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using UIKit;

using Xamarin.TTTAttributedLabel;

namespace NomadCode.BotFramework.iOS
{
	public class MessageCell : UITableViewCell, ITTTAttributedLabelDelegate
	{
		public static readonly nfloat StackViewPadding = 5;

		public static readonly nfloat AutoCompleteHeight = 50;

		public static readonly nfloat ContentWidth = UIScreen.MainScreen.Bounds.Width - 49;


		public static readonly CGSize AvatarRatio = new CGSize (1, 1);
		public static readonly CGSize AvatarImageSize = new CGSize (24, (AvatarRatio.Height / AvatarRatio.Width) * 24);
		public static readonly CGSize AvatarScaledImageSize = new CGSize (AvatarImageSize.Width * UIScreen.MainScreen.Scale, AvatarImageSize.Height * UIScreen.MainScreen.Scale);

		public static readonly CGSize HeroRatio = new CGSize (5, 4);
		public static readonly CGSize HeroImageSize = new CGSize (ContentWidth, (HeroRatio.Height / HeroRatio.Width) * ContentWidth);
		public static readonly CGSize HeroScaledImageSize = new CGSize (HeroImageSize.Width * UIScreen.MainScreen.Scale, HeroImageSize.Height * UIScreen.MainScreen.Scale);

		public static readonly CGSize ThumbnailRatio = new CGSize (5, 4);
		public static readonly CGSize ThumbnailImageSize = new CGSize (ContentWidth, (ThumbnailRatio.Height / ThumbnailRatio.Width) * ContentWidth);
		public static readonly CGSize ThumbnailScaledImageSize = new CGSize (ThumbnailImageSize.Width * UIScreen.MainScreen.Scale, ThumbnailImageSize.Height * UIScreen.MainScreen.Scale);


		#region Views

		UIStackView _contentStackView;

		TTTAttributedLabel _bodyLabel;

		UILabel _titleLabel, _timestampLabel;

		UIImageView _avatarView, _thumbnailImageView;

		NSMutableDictionary constraintViews = new NSMutableDictionary ();

		NSMutableDictionary constraintMetrics = NSMutableDictionary.FromObjectsAndKeys (
			new NSNumber [] { NSNumber.FromNFloat (AvatarImageSize.Height), NSNumber.FromNFloat (13), NSNumber.FromNFloat (10), NSNumber.FromNFloat (5), NSNumber.FromNFloat (AvatarImageSize.Height + 15) },
			new NSString [] { new NSString (@"avatarSize"), new NSString (@"padding"), new NSString (@"right"), new NSString (@"left"), new NSString (@"leftInset") }
		);

		static NSAttributedString emptyAttributedString = new NSAttributedString (string.Empty);

		public List<UIImageView> HeroImageViews = new List<UIImageView> ();

		public override UIImageView ImageView => AvatarView;

		public UIStackView ContentStackView => _contentStackView ?? (_contentStackView = MessageCellSubviews.GetContentStackView ());

		public TTTAttributedLabel BodyLabel => _bodyLabel ?? (_bodyLabel = MessageCellSubviews.GetBodyLabel (this));

		public UILabel TitleLabel => _titleLabel ?? (_titleLabel = MessageCellSubviews.GetTitleLabel ());

		public UILabel TimestampLabel => _timestampLabel ?? (_timestampLabel = MessageCellSubviews.GetTimestampLabel ());

		public UIImageView AvatarView => _avatarView ?? (_avatarView = MessageCellSubviews.GetAvatarView ());

		public UIImageView ThumbnailImageView => _thumbnailImageView ?? (_thumbnailImageView = MessageCellSubviews.GetThumbnailImageView ());

		#endregion

		NSIndexPath _indexPath;
		public NSIndexPath IndexPath
		{
			get => _indexPath;
			set
			{
				_indexPath = value;
				Tag = _indexPath.Row;
			}
		}


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

			BodyLabel.SetText (emptyAttributedString);

			if (IsHeaderCell)
			{
				TitleLabel.Font = AttributedStringUtilities.HeaderFont;
				TimestampLabel.Font = AttributedStringUtilities.TimestampFont;

				TitleLabel.Text = string.Empty;
				TimestampLabel.Text = string.Empty;
			}

			var views = ContentStackView.ArrangedSubviews;

			foreach (var view in views)
			{
				ContentStackView.RemoveArrangedSubview (view);
				view.RemoveFromSuperview ();
				//TODO: Cleanup actions and view
			}

			if (HeroImageViews?.Count > 0)
			{
				HeroImageViews = new List<UIImageView> ();
			}

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}


		public void SetHeader (DateTime? timestamp, string username)
		{
			TitleLabel.Text = username;
			TimestampLabel.Text = timestamp?.ToShortTimeString ();
		}

		public void SetAvatar (int key, UIImage avatar) => AvatarView.Image = key == Tag ? avatar : null;


		public void SetMessage (NSAttributedString message) => BodyLabel.SetText (message);


		public void AddAttachmentTitle (NSAttributedString text)
		{
			var label = MessageCellSubviews.GetAttachmentTitleLabel (this);
			label.SetText (text);
			ContentStackView.AddArrangedSubview (label);
		}


		public void AddAttachmentSubtitle (NSAttributedString text)
		{
			var label = MessageCellSubviews.GetAttachmentSubtitleLabel (this);
			label.SetText (text);
			ContentStackView.AddArrangedSubview (label);
		}


		public void AddAttachmentText (NSAttributedString text)
		{
			var label = MessageCellSubviews.GetBodyLabel (this);
			label.SetText (text);
			ContentStackView.AddArrangedSubview (label);
		}


		public int AddHeroImage ()
		{
			var imageCount = HeroImageViews.Count;

			var image = MessageCellSubviews.GetHeroImageView ();

			image.AddConstraint (NSLayoutConstraint.Create (image, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 0, HeroImageSize.Height));
			image.AddConstraint (NSLayoutConstraint.Create (image, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 0, HeroImageSize.Width));

			ContentStackView.AddArrangedSubview (image);

			HeroImageViews.Add (image);

			return imageCount;
		}

		public void SetHeroImage (int key, int index, UIImage image) => HeroImageViews [index].Image = key == Tag ? image : null;


		public void AddThumbnailImage () => ContentStackView.AddArrangedSubview (ThumbnailImageView);

		public void SetThumbnailImage (int key, UIImage image) => ThumbnailImageView.Image = key == Tag ? image : null;


		public void SetButtons (params (string Title, string Value) [] buttons)
		{
			foreach (var button in buttons)
			{
				ContentStackView.AddArrangedSubview (MessageCellSubviews.GetButton (button.Title));
			}
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


			ContentView.AddSubview (ContentStackView);

			constraintViews.Add (new NSString (@"stackView"), ContentStackView);


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