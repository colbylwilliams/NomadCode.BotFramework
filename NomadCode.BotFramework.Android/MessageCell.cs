
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace NomadCode.BotFramework.Droid
{
	public class MessageCell : ViewGroup
	{
		static int _contentWidth;

		public static int ContentWidth
		{
			get
			{
				if (_contentWidth == 0)
				{
					var size = new Point ();

					if (Application.Context.GetSystemService (Context.WindowService) is IWindowManager windowManager)
					{
						Log.Debug ($"{windowManager}");

						windowManager.DefaultDisplay.GetRealSize (size);

						_contentWidth = size.X - 49;
					}
					else
					{
						Log.Error ("Could not cast windowManager");
					}
				}

				return _contentWidth;
			}
		}

		public static readonly Size AvatarRatio = new Size (1, 1);
		public static readonly Size AvatarImageSize = new Size (24, (AvatarRatio.Height / AvatarRatio.Width) * 24);
		public static readonly Size AvatarScaledImageSize = new Size (AvatarImageSize.Width /** UIScreen.MainScreen.Scale*/, AvatarImageSize.Height /** UIScreen.MainScreen.Scale*/);

		public static readonly Size HeroRatio = new Size (5, 4);
		public static readonly Size HeroImageSize = new Size (ContentWidth, (HeroRatio.Height / HeroRatio.Width) * ContentWidth);
		public static readonly Size HeroScaledImageSize = new Size (HeroImageSize.Width /** UIScreen.MainScreen.Scale*/, HeroImageSize.Height /** UIScreen.MainScreen.Scale*/);

		public static readonly Size ThumbnailRatio = new Size (5, 4);
		public static readonly Size ThumbnailImageSize = new Size (ContentWidth, (ThumbnailRatio.Height / ThumbnailRatio.Width) * ContentWidth);
		public static readonly Size ThumbnailScaledImageSize = new Size (ThumbnailImageSize.Width /** UIScreen.MainScreen.Scale*/, ThumbnailImageSize.Height /** UIScreen.MainScreen.Scale*/);


		#region Views

		LinearLayout _contentStackView;

		TextView _bodyLabel;

		TextView _titleLabel, _timestampLabel;

		ImageView _avatarView, _thumbnailImageView;

		//NSMutableDictionary constraintViews = new NSMutableDictionary ();

		//NSMutableDictionary constraintMetrics = NSMutableDictionary.FromObjectsAndKeys (
		//	new NSNumber [] { NSNumber.FromNFloat (AvatarImageSize.Height), NSNumber.FromNFloat (13), NSNumber.FromNFloat (10), NSNumber.FromNFloat (5), NSNumber.FromNFloat (AvatarImageSize.Height + 15) },
		//	new NSString [] { new NSString (@"avatarSize"), new NSString (@"padding"), new NSString (@"right"), new NSString (@"left"), new NSString (@"leftInset") }
		//);

		static string emptyAttributedString = string.Empty;

		public List<ImageView> HeroImageViews = new List<ImageView> ();

		//public override ImageView ImageView => AvatarView;

		public LinearLayout ContentStackView => _contentStackView ?? (_contentStackView = MessageCellSubviews.GetContentStackView (Context));

		public TextView BodyLabel => _bodyLabel ?? (_bodyLabel = MessageCellSubviews.GetBodyLabel (Context));

		public TextView TitleLabel => _titleLabel ?? (_titleLabel = MessageCellSubviews.GetTitleLabel (Context));

		public TextView TimestampLabel => _timestampLabel ?? (_timestampLabel = MessageCellSubviews.GetTimestampLabel (Context));

		public ImageView AvatarView => _avatarView ?? (_avatarView = MessageCellSubviews.GetAvatarView (Context));

		public ImageView ThumbnailImageView => _thumbnailImageView ?? (_thumbnailImageView = MessageCellSubviews.GetThumbnailImageView (Context));

		public LinearLayout ContentView { get; set; }

		#endregion

		public int Key;


		bool? _isHeaderCell;

		public bool IsHeaderCell => _isHeaderCell ?? (_isHeaderCell = ViewType > 0).Value;

		public int ViewType { get; set; }


		public MessageCell (Context context, int viewType) :
			base (context)
		{
			ViewType = viewType;

			ContentView = new LinearLayout (context) { Orientation = Orientation.Vertical };

			configureViewsForCellType ();
		}


		public override bool ShouldDelayChildPressedState () => false;


		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			// Measurement will ultimately be computing these values.
			int maxHeight = 0;
			int maxWidth = 0;
			int childState = 0;

			// Iterate through all children, measuring them and computing our dimensions
			// from their size.
			for (int i = 0; i < ChildCount; i++)
			{
				var child = GetChildAt (i);

				if (child.Visibility != ViewStates.Gone)
				{
					// Measure the child.
					MeasureChildWithMargins (child, widthMeasureSpec, 0, heightMeasureSpec, 0);

					maxWidth = Math.Max (maxWidth, child.MeasuredWidth);

					maxHeight = Math.Max (maxHeight, child.MeasuredHeight);

					childState = CombineMeasuredStates (childState, child.MeasuredState);
				}
			}

			// Check against our minimum height and width
			maxHeight = Math.Max (maxHeight, SuggestedMinimumHeight);
			maxWidth = Math.Max (maxWidth, SuggestedMinimumWidth);

			// Report our final dimensions.
			SetMeasuredDimension (ResolveSizeAndState (maxWidth, widthMeasureSpec, childState),
								  ResolveSizeAndState (maxHeight, heightMeasureSpec, childState << MeasuredHeightStateShift));

			//base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
		}

		Rect mTmpContainerRect = new Rect ();
		Rect mTmpChildRect = new Rect ();

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			// These are the far left and right edges in which we are performing layout.
			int leftPos = PaddingLeft;
			int rightPos = r - l - PaddingRight;

			// These are the top and bottom edges in which we are performing layout.
			var parentTop = PaddingTop;
			var parentBottom = b - t - PaddingBottom;

			for (int i = 0; i < ChildCount; i++)
			{
				var child = GetChildAt (i);

				if (child.Visibility != ViewStates.Gone)
				{
					var lp = child.LayoutParameters;

					var width = child.MeasuredWidth;
					var height = child.MeasuredHeight;

					// Compute the frame in which we are placing this child.
					mTmpContainerRect.Left = leftPos;
					mTmpContainerRect.Right = rightPos;

					mTmpContainerRect.Top = parentTop;
					mTmpContainerRect.Bottom = parentBottom;

					// Use the child's gravity and size to determine its final
					// frame within its container.
					Gravity.Apply (GravityFlags.Start | GravityFlags.CenterVertical, width, height, mTmpContainerRect, mTmpChildRect);

					// Place the child.
					child.Layout (mTmpChildRect.Left, mTmpChildRect.Top, mTmpChildRect.Right, mTmpChildRect.Bottom);
				}
			}
		}


		public void PrepareForReuse ()
		{
			BodyLabel.Text = null;

			if (IsHeaderCell)
			{
				//TitleLabel.Font = AttributedStringUtilities.HeaderFont;
				//TimestampLabel.Font = AttributedStringUtilities.TimestampFont;

				TitleLabel.Text = string.Empty;
				TimestampLabel.Text = string.Empty;
			}

			ContentStackView.RemoveAllViewsInLayout ();

			//TODO: Cleanup actions and view
			//foreach (var view in views)
			//{
			//	ContentStackView.RemoveArrangedSubview (view);
			//	view.RemoveFromSuperview ();
			//}

			if (HeroImageViews?.Count > 0)
			{
				HeroImageViews = new List<ImageView> ();
			}

			//SelectionStyle = UITableViewCellSelectionStyle.None;
		}


		public void SetHeader (DateTime? timestamp, string username)
		{
			TitleLabel.Text = username;
			TimestampLabel.Text = timestamp?.ToShortTimeString ();
		}


		public void SetAvatar (int key, Bitmap avatar) => AvatarView.SetImageBitmap (key == Key ? avatar : null);


		public void SetMessage (Java.Lang.ICharSequence message) => BodyLabel.SetText (message, TextView.BufferType.Spannable);


		public void AddAttachmentTitle (Java.Lang.ICharSequence text)
		{
			var label = MessageCellSubviews.GetAttachmentTitleLabel (Context);
			label.SetText (text, TextView.BufferType.Spannable);
			ContentStackView.AddView (label);
		}


		public void AddAttachmentSubtitle (Java.Lang.ICharSequence text)
		{
			var label = MessageCellSubviews.GetAttachmentSubtitleLabel (Context);
			label.SetText (text, TextView.BufferType.Spannable);
			ContentStackView.AddView (label);
		}


		public void AddAttachmentText (Java.Lang.ICharSequence text)
		{
			var label = MessageCellSubviews.GetBodyLabel (Context);
			label.SetText (text, TextView.BufferType.Spannable);
			ContentStackView.AddView (label);
		}


		public int AddHeroImage ()
		{
			var imageCount = HeroImageViews.Count;

			var image = MessageCellSubviews.GetHeroImageView (Context);

			//image.AddConstraint (NSLayoutConstraint.Create (image, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 0, HeroImageSize.Height));
			//image.AddConstraint (NSLayoutConstraint.Create (image, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 0, HeroImageSize.Width));

			ContentStackView.AddView (image);

			HeroImageViews.Add (image);

			return imageCount;
		}

		public void SetHeroImage (int key, int index, Bitmap image) => HeroImageViews [index].SetImageBitmap (key == Key ? image : null);


		public void AddThumbnailImage () => ContentStackView.AddView (ThumbnailImageView);

		public void SetThumbnailImage (int key, Bitmap image) => ThumbnailImageView.SetImageBitmap (key == Key ? image : null);


		public void SetButtons (params (string Title, string Value) [] buttons)
		{
			foreach (var button in buttons)
			{
				ContentStackView.AddView (MessageCellSubviews.GetButton (Context, button.Title));
			}
		}


		//[Export ("attributedLabel:didSelectLinkWithURL:")]
		//public void DidSelectLinkWithURL (TTTAttributedLabel label, NSUrl url)
		//{
		//	Console.WriteLine ($"DidSelectLinkWithURL Label = {label}, Url = {url})");
		//}


		void configureViewsForCellType ()
		{
			if (IsHeaderCell)
			{
				configureViewsForHeaderCell ();
			}

			ContentView.AddView (BodyLabel);

			//constraintViews.Add (new NSString (@"contentView"), BodyLabel);


			ContentView.AddView (ContentStackView);

			//constraintViews.Add (new NSString (@"stackView"), ContentStackView);


			configureConstraintsForCellContent ();


			void configureViewsForHeaderCell ()
			{
				ContentView.AddView (AvatarView);
				ContentView.AddView (TitleLabel);
				ContentView.AddView (TimestampLabel);

				//constraintViews.Add (new NSString (@"avatarView"), AvatarView);
				//constraintViews.Add (new NSString (@"titleLabel"), TitleLabel);
				//constraintViews.Add (new NSString (@"timestampLabel"), TimestampLabel);

				//configureConstraintsForHeaderCell ();

				//void configureConstraintsForHeaderCell ()
				//{
				//	var hConstraints = @"H:|-left-[avatarView(avatarSize)]-right-[titleLabel(>=0)]-left-[timestampLabel]-(>=right)-|";

				//	var vConstraints = @"V:|-padding-[avatarView(avatarSize)]-(>=0)-|";

				//	ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (hConstraints, 0, constraintMetrics, constraintViews));

				//	ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (vConstraints, 0, constraintMetrics, constraintViews));

				//	ContentView.AddConstraint (NSLayoutConstraint.Create (TitleLabel, NSLayoutAttribute.Baseline, NSLayoutRelation.Equal, TimestampLabel, NSLayoutAttribute.Baseline, 1, 0.5f));
				//}
			}

			void configureConstraintsForCellContent ()
			{
				//var sConstraints = @"H:|-leftInset-[stackView(>=0)]-right-|";

				//var hConstraints = @"H:|-leftInset-[contentView(>=0)]-right-|";

				//var vConstraints = IsHeaderCell ? @"V:|-right-[titleLabel]-left-[contentView]-left-[stackView]-(>=0@999)-|" : @"V:|[contentView]-left-[stackView]-(>=0@999)-|";

				//ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (hConstraints, 0, constraintMetrics, constraintViews));

				//ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (vConstraints, 0, constraintMetrics, constraintViews));

				//ContentView.AddConstraints (NSLayoutConstraint.FromVisualFormat (sConstraints, 0, constraintMetrics, constraintViews));
			}
		}
	}
}
