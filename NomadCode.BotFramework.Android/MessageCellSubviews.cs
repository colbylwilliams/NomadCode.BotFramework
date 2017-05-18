using Android.Content;
using Android.Widget;

namespace NomadCode.BotFramework.Droid
{
	public static class MessageCellSubviews
	{
		public static TextView GetBodyLabel (Context context)
		{
			var bodyLabel = new TextView (context)
			{

			};

			bodyLabel.SetTextColor (Colors.MessageColor);
			bodyLabel.SetLinkTextColor (Colors.MessageLinkColor);

			return bodyLabel;
		}


		public static TextView GetTitleLabel (Context context)
		{
			var titleLabel = new TextView (context)
			{

			};

			titleLabel.SetTextColor (Colors.MessageColor);

			return titleLabel;
		}


		public static TextView GetAttachmentTitleLabel (Context context)
		{
			var titleLabel = new TextView (context)
			{

			};

			titleLabel.SetTextColor (Colors.MessageColor);
			titleLabel.SetLinkTextColor (Colors.MessageLinkColor);

			return titleLabel;
		}


		public static TextView GetAttachmentSubtitleLabel (Context context)
		{
			var subTitleLabel = new TextView (context)
			{

			};

			subTitleLabel.SetTextColor (Android.Graphics.Color.DarkGray);
			subTitleLabel.SetLinkTextColor (Colors.MessageLinkColor);

			return subTitleLabel;
		}


		public static TextView GetTimestampLabel (Context context)
		{
			var timestampLabel = new TextView (context)
			{

			};

			timestampLabel.SetTextColor (Android.Graphics.Color.LightGray);

			return timestampLabel;
		}


		public static ImageView GetAvatarView (Context context)
		{
			var avatarView = new ImageView (context)
			{

			};

			return avatarView;
		}


		public static ImageView GetHeroImageView (Context context)
		{
			var heroImageView = new ImageView (context)
			{

			};

			return heroImageView;
		}


		public static ImageView GetThumbnailImageView (Context context)
		{
			var thumbnailImageView = new ImageView (context)
			{

			};

			return thumbnailImageView;
		}


		public static Button GetButton (Context context, string title)
		{
			var button = new Button (context)
			{
				Text = title
			};

			button.SetTextColor (Android.Graphics.Color.DarkGray);

			return button;
		}


		public static LinearLayout GetContentStackView (Context context)
		{
			var stackView = new LinearLayout (context)
			{

			};

			return stackView;
		}
	}
}
