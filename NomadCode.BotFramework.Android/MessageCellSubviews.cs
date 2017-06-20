using Android.Content;
using Android.Widget;
using Android.Graphics;

namespace NomadCode.BotFramework.Droid
{
	public static class MessageCellSubviews
	{
		public static TextView GetBodyLabel (MessageCell cell)
		{
			var bodyLabel = new TextView (cell.Context)
			{

			};

			bodyLabel.SetTextColor (Color.White /*Colors.MessageColor*/);
			bodyLabel.SetLinkTextColor (Colors.MessageLinkColor);

			bodyLabel.LayoutParameters = cell.LayoutParameters;

			return bodyLabel;
		}


		public static TextView GetTitleLabel (MessageCell cell)
		{
			var titleLabel = new TextView (cell.Context)
			{

			};

			titleLabel.SetTextColor (Color.White /*Colors.MessageColor*/);

			titleLabel.LayoutParameters = cell.LayoutParameters;

			return titleLabel;
		}


		public static TextView GetAttachmentTitleLabel (MessageCell cell)
		{
			var titleLabel = new TextView (cell.Context)
			{

			};

			titleLabel.SetTextColor (Color.White /*Colors.MessageColor*/);
			titleLabel.SetLinkTextColor (Colors.MessageLinkColor);

			titleLabel.LayoutParameters = cell.LayoutParameters;

			return titleLabel;
		}


		public static TextView GetAttachmentSubtitleLabel (MessageCell cell)
		{
			var subTitleLabel = new TextView (cell.Context)
			{

			};

			subTitleLabel.SetTextColor (Color.GhostWhite /*Color.DarkGray*/);
			subTitleLabel.SetLinkTextColor (Colors.MessageLinkColor);

			subTitleLabel.LayoutParameters = cell.LayoutParameters;

			return subTitleLabel;
		}


		public static TextView GetTimestampLabel (MessageCell cell)
		{
			var timestampLabel = new TextView (cell.Context)
			{

			};

			timestampLabel.SetTextColor (Color.WhiteSmoke /*Color.LightGray*/);

			timestampLabel.LayoutParameters = cell.LayoutParameters;

			return timestampLabel;
		}


		public static ImageView GetAvatarView (MessageCell cell)
		{
			var avatarView = new ImageView (cell.Context)
			{

			};

			avatarView.LayoutParameters = cell.LayoutParameters;

			return avatarView;
		}


		public static ImageView GetHeroImageView (MessageCell cell)
		{
			var heroImageView = new ImageView (cell.Context)
			{

			};

			heroImageView.LayoutParameters = cell.LayoutParameters;

			return heroImageView;
		}


		public static ImageView GetThumbnailImageView (MessageCell cell)
		{
			var thumbnailImageView = new ImageView (cell.Context)
			{

			};

			thumbnailImageView.LayoutParameters = cell.LayoutParameters;

			return thumbnailImageView;
		}


		public static Button GetButton (MessageCell cell, string title)
		{
			var button = new Button (cell.Context)
			{
				Text = title
			};

			button.SetTextColor (Color.DarkGray);

			button.LayoutParameters = cell.LayoutParameters;

			return button;
		}


		public static LinearLayout GetContentStackView (MessageCell cell)
		{
			var stackView = new LinearLayout (cell.Context)
			{

			};

			stackView.LayoutParameters = cell.LayoutParameters;

			return stackView;
		}
	}
}
