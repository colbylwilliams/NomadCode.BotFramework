using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

                var reuseId = message.Head ? MessageCellReuseIds.MessageHeaderCellReuseId : MessageCellReuseIds.MessageCellReuseId;

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
                        if (attachment.Content.HasTitle)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        if (attachment.Content.HasSubtitle)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        if (attachment.Content.HasImages)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        // buttons
                        if (attachment.Content.HasButtons)
                        {
                            cell.SetButtons (attachment.Content.Buttons.Select (b => (b.Title, b.Value.ToString ())).ToArray ());
                        }
                    }
                }

                // Cells must inherit the table view's transform
                // This is very important, since the main table view may be inverted
                cell.Transform = tableView.Transform;

                //Log.Debug($"{cell.BodyLabel.Bounds.Width}");

                return cell;
            }

            return null;
        }


        static void SetHeroImage (this MessageCell cell, NSIndexPath indexPath, string imageUrl)
        {
            cell.HeroImageView.SetCacheFormat (getCacheFormat (MessageCell.HeroImageSize));

            cell.AddHeroImage ();

            if (string.IsNullOrEmpty (imageUrl))
            {
                cell.SetHeroImage (indexPath.Row, null);
            }
            else
            {
                var placeholder = getPlaceholderImage (MessageCell.HeroImageSize);

                using (NSUrl url = new NSUrl (imageUrl))
                {
                    cell?.HeroImageView.SetImage (url, placeholder, (img) => cell.SetHeroImage (indexPath.Row, img), (err) =>
                    {
                        cell.SetHeroImage (indexPath.Row, null);

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
            cell.SetHeader (message.LocalTimeStamp, message.Activity?.From?.Name);

            cell.ImageView.SetCacheFormat (getCacheFormat (MessageCell.AvatarImageSize));

            if (message.Activity.From.Id == "DigitalAgencies")
            {
                cell.SetAvatar (indexPath.Row, UIImage.FromBundle ("avatar_microsoft"));
            }
            else
            {
                var avatarUrl = string.IsNullOrEmpty (message.Activity?.From?.Id) ? string.Empty : BotClient.Shared.GetAvatarUrl (message.Activity.From.Id);

                if (string.IsNullOrEmpty (avatarUrl))
                {
                    cell.SetAvatar (indexPath.Row, null);
                }
                else
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
            }
        }


        static Dictionary<(nfloat Width, nfloat Height), UIImage> placeholders = new Dictionary<(nfloat Width, nfloat Height), UIImage> ();

        static UIImage getPlaceholderImage ((nfloat Width, nfloat Height) size)
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


        static HNKCacheFormat getCacheFormat ((nfloat Width, nfloat Height) size)
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


        public static MessageCell GetAutoCompleteCell (this List<(string Id, string Name)> items, UITableView tableView, NSIndexPath indexPath)
        {
            if (items?.Count > 0 && items.Count > indexPath.Row)
            {
                //Log.Debug ($"GetAutoCompleteCell = [{indexPath}]");

                var cell = tableView.DequeueReusableCell (MessageCellReuseIds.AutoCompleteReuseId, indexPath) as MessageCell;
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


        public static nfloat GetMessageHeight (this List<BotMessage> messages, UITableView tableView, NSIndexPath indexPath)
        {
            var row = indexPath.Row;

            //Log.Debug ($"table: {tableView.Frame.Width - 49}, contentWidth: {MessageCell.ContentWidth}");

            if (messages?.Count > 0 && messages.Count > row)
            {
                var message = messages [row];

                nfloat height = message.CellHeight;

                if (height > 0)
                {
                    return height;
                }

                message.Head = row == messages.Count - 1 || (row + 1 < messages.Count) && (messages [row + 1].Activity.From.Name != message.Activity.From.Name);

                var bodyBounds = message.HasText ? message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;

                height = bodyBounds.Height + 5;// + 8.5f; // empty stackView = 3.5f + bottom padding = 5

                //Log.Debug($"{width}");

                if (message.Head) height += 36.5f; // pading(10) + title(21.5) + padding(5) + content(height)


                if (message.HasAtachments)
                {
                    foreach (var attachment in message.Attachments)
                    {
                        if (attachment.Content.HasTitle)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        if (attachment.Content.HasSubtitle)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        if (attachment.Content.HasImages)
                        {
                            //var attachmentTitleBounds = message.AttributedText.GetBoundingRect (new CGSize (MessageCell.ContentWidth, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin, null) : CGRect.Empty;
                        }

                        // buttons
                        if (attachment.Content.HasButtons)
                        {
                            height += (32 * attachment.Content.Buttons.Count);
                            height += 4 * (attachment.Content.Buttons.Count - 1);
                            height += 5;
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
