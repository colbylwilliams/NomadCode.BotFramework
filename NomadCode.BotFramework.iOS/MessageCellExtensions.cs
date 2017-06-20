using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using UIKit;

using Haneke;
using System.Linq;

namespace NomadCode.BotFramework.iOS
{
	public static class MessageCellExtensions
	{
		public static MessageCell GetMessageCell (this List<BotMessage> messages, UITableView tableView, NSIndexPath indexPath)
		{
			var row = indexPath.Row;

			if (messages?.Count > 0 && messages.Count > row)
			{
				var message = messages [row];

				var reuseId = message.Head ? MessageCell.MessageHeaderCellReuseId : MessageCell.MessageCellReuseId;

				var cell = tableView.DequeueReusableCell (reuseId, indexPath) as MessageCell;

				cell.IndexPath = indexPath;

				if (message.Head)
				{
					cell.SetHeader (indexPath, message);
				}

				if (message.HasText)
				{
					cell.SetMessage (message.AttributedText);
				}

				if (message.HasAtachments)
				{
					foreach (var attachment in message.Attachments)
					{
						// card images
						if (attachment.Content.HasImages)
						{
							foreach (var image in attachment.Content.Images)
							{
								cell.AddHeroImage (indexPath, image.Url);
							}
						}

						// raw attachment images
						if (attachment.Attachment.ContentType == CardContentTypes.ImageJpeg)
						{
							cell.AddHeroImage (indexPath, attachment.Attachment.ContentUrl);
						}

						// title
						if (attachment.Content.HasTitle)
						{
							cell.AddAttachmentTitle (attachment.Content.AttributedTitle);
						}

						// subtitle
						if (attachment.Content.HasSubtitle)
						{
							cell.AddAttachmentSubtitle (attachment.Content.AttributedSubtitle);
						}

						// text
						if (attachment.Content.HasText)
						{
							cell.AddAttachmentText (attachment.Content.AttributedText);
						}

						// buttons
						if (attachment.Content.HasButtons)
						{
							//cell.SetButtons (attachment.Content.Buttons.Select (b => (b.Title, b.Value.ToString ())).ToArray ());
							cell.SetButtons (attachment.Content.Buttons.ToArray ());
						}
					}
				}

				// Cells must inherit the table view's transform
				// This is very important, since the main table view may be inverted
				cell.Transform = tableView.Transform;

				return cell;
			}

			return null;
		}


		static void AddHeroImage (this MessageCell cell, NSIndexPath indexPath, string imageUrl)
		{
			var index = cell.AddHeroImage ();

			cell.HeroImageViews [index].SetCacheFormat (getCacheFormat (MessageCell.HeroScaledImageSize));

			if (string.IsNullOrEmpty (imageUrl))
			{
				cell.SetHeroImage (indexPath.Row, index, null);
			}
			else
			{
				var placeholder = getPlaceholderImage (MessageCell.HeroScaledImageSize);

				using (NSUrl url = new NSUrl (imageUrl))
				{
					cell?.HeroImageViews [index].SetImage (url, placeholder, (img) => cell.SetHeroImage (indexPath.Row, index, img), (err) =>
					{
						cell.SetHeroImage (indexPath.Row, index, null);

						Log.Debug (err.LocalizedDescription);
					});
				}
			}
		}


		static void SetThumbnailImage (this MessageCell cell, NSIndexPath indexPath, string imageUrl)
		{
			cell.ThumbnailImageView.SetCacheFormat (getCacheFormat (MessageCell.ThumbnailImageSize));

			cell.AddThumbnailImage ();

			if (string.IsNullOrEmpty (imageUrl))
			{
				cell.SetThumbnailImage (indexPath.Row, null);
			}
			else
			{
				var placeholder = getPlaceholderImage (MessageCell.ThumbnailImageSize);

				using (NSUrl url = new NSUrl (imageUrl))
				{
					cell?.ThumbnailImageView.SetImage (url, placeholder, (img) => cell.SetThumbnailImage (indexPath.Row, img), (err) =>
					{
						cell.SetThumbnailImage (indexPath.Row, null);

						Log.Debug (err.LocalizedDescription);
					});
				}
			}
		}


		static void SetHeader (this MessageCell cell, NSIndexPath indexPath, BotMessage message)
		{
			var username = message.Activity?.From?.Name ?? (string.Compare (message.Activity?.From?.Id, BotClient.Shared.CurrentUserId, true) == 0 ? BotClient.Shared.CurrentUserName : "  ");

			if (string.Compare (username, "cisbot-prod-svc", true) == 0)
			{
				username = "Litware Bot";
			}

			cell.SetHeader (message.LocalTimeStamp, username);

			cell.ImageView.SetCacheFormat (getCacheFormat (MessageCell.AvatarImageSize));

			var avatarUrl = string.IsNullOrEmpty (message.Activity?.From?.Id) ? string.Empty : BotClient.Shared.GetAvatarUrl (message.Activity.From.Id);

			if (string.IsNullOrEmpty (avatarUrl))
			{
				cell.SetAvatar (indexPath.Row, null);
			}
			else if (avatarUrl.Contains ("http"))
			{
				var placeholder = getPlaceholderImage (MessageCell.AvatarImageSize);

				using (NSUrl url = new NSUrl (avatarUrl))
				{
					cell?.ImageView.SetImage (url, placeholder, (img) => cell.SetAvatar (indexPath.Row, img), (err) =>
					{
						cell.SetAvatar (indexPath.Row, null);

						Log.Debug (err.LocalizedDescription);
					});
				}
			}
			else
			{
				cell.SetAvatar (indexPath.Row, UIImage.FromBundle (avatarUrl));
			}
		}


		static Dictionary<CGSize, UIImage> placeholders = new Dictionary<CGSize, UIImage> ();

		static UIImage getPlaceholderImage (CGSize size)
		{
			if (!placeholders.TryGetValue (size, out UIImage placeholder))
			{
				UIGraphics.BeginImageContextWithOptions (new CGSize (size.Width, size.Height), false, 0);

				placeholder = UIGraphics.GetImageFromCurrentImageContext ();

				UIGraphics.EndImageContext ();

				placeholders [size] = placeholder;
			}

			return placeholder;
		}


		static HNKCacheFormat getCacheFormat (CGSize size)
		{
			var name = $"avatar{size.GetHashCode ()}";

			var format = HNKCache.SharedCache.Formats [name] as HNKCacheFormat;

			if (format == null)
			{
				format = new HNKCacheFormat (name)
				{
					Size = new CGSize (size.Width, size.Height),
					ScaleMode = HNKScaleMode.AspectFit,
					DiskCapacity = 10 * 1024 * 1024, // 10MB
					PreloadPolicy = HNKPreloadPolicy.LastSession
				};
			}

			return format;
		}


		public static MessageCell GetAutoCompleteCell (this List<ISearchResults> items, UITableView tableView, NSIndexPath indexPath)
		{
			if (items?.Count > 0 && items.Count > indexPath.Row)
			{
				//Log.Debug ($"GetAutoCompleteCell = [{indexPath}]");

				var cell = tableView.DequeueReusableCell (MessageCell.AutoCompleteReuseId, indexPath) as MessageCell;
				cell.IndexPath = indexPath;

				var text = items [indexPath.Row].Name;

				//if (FoundPrefix.Equals (hashStr))
				//{
				//  text = $"# {text}";
				//}
				//else if (FoundPrefix.Equals (colonStr) || FoundPrefix.Equals (plusColonStr))
				//{
				//  text = $":{text}:";
				//}

				cell.TitleLabel.Text = text;
				cell.SelectionStyle = UITableViewCellSelectionStyle.Default;

				return cell;
			}

			return null;
		}


		public static nfloat GetMessageHeight (this List<BotMessage> messages, NSIndexPath indexPath)
		{
			var row = indexPath.Row;

			//Log.Debug ($"table: {tableView.Frame.Width - 49}, contentWidth: {MessageCell.ContentWidth}");

			if (messages?.Count > 0 && messages.Count > row)
			{
				var message = messages [row];

				nfloat height = message.CellHeight;

				//if (height > 0)
				//{
				//	return height;
				//}

				message.Head = messages.IsHead (row);

				var bodyBounds = message.HasText ? message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;

				height = bodyBounds.Height + MessageCell.StackViewPadding;// + 8.5f; // empty stackView = 3.5f + bottom padding = 5

				//Log.Debug($"{width}");

				if (message.Head) height += 36.5f; // pading(10) + title(21.5) + padding(5) + content(height)


				if (message.HasAtachments)
				{
					foreach (var attachment in message.Attachments)
					{
						// card images
						if (attachment.Content.HasImages)
						{
							foreach (var image in attachment.Content.Images)
							{
								height += MessageCell.HeroImageSize.Height + MessageCell.StackViewPadding;
							}
						}

						// raw attachment images
						if (attachment.Attachment.ContentType == CardContentTypes.ImageJpeg)
						{
							height += MessageCell.HeroImageSize.Height + MessageCell.StackViewPadding;
						}

						// title
						if (attachment.Content.HasTitle)
						{
							var attachmentTitleBounds = attachment.Content.AttributedTitle.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null);
							height += attachmentTitleBounds.Height + MessageCell.StackViewPadding;
						}

						// subtitle
						if (attachment.Content.HasSubtitle)
						{
							var attachmentSubtitleBounds = attachment.Content.AttributedSubtitle.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null);
							height += attachmentSubtitleBounds.Height + MessageCell.StackViewPadding;
						}

						// text
						if (attachment.Content.HasText)
						{
							var attachmentTextBounds = attachment.Content.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null);
							height += attachmentTextBounds.Height + MessageCell.StackViewPadding;
						}

						// buttons
						if (attachment.Content.HasButtons)
						{
							height += (32 * attachment.Content.Buttons.Count);
							height += 4 * (attachment.Content.Buttons.Count - 1);
							height += MessageCell.StackViewPadding;
						}
					}
				}

				message.CellHeight = height;

				return height;
			}

			return 0;
		}
	}
}
