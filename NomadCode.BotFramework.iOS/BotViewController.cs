using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

using SlackHQ;
using AVFoundation;
using System.IO;

namespace NomadCode.BotFramework.iOS
{
	public interface ISearchResults
	{
		string Id { get; set; }
		string Name { get; set; }
	}

	[Register ("BotViewController")]
	public class BotViewController : SlackTextViewController, IUIImagePickerControllerDelegate
	{
		UIWindow pipWindow;

		List<BotMessage> Messages => BotClient.Shared.Messages;

		List<ISearchResults> searchResult = new List<ISearchResults> ();


		#region ViewController Lifecycle

		[Export ("initWithCoder:")]
		public BotViewController (NSCoder coder) : base (coder) => commonInit ();


		public BotViewController (IntPtr handle) : base (handle) => commonInit ();


		void commonInit ()
		{
			NSNotificationCenter.DefaultCenter.AddObserver (TableView, new Selector ("reloadData"), UIApplication.ContentSizeCategoryChangedNotification, null);
			NSNotificationCenter.DefaultCenter.AddObserver (this, new Selector ("textInputbarDidMove:"), SlackTextInputbar.DidMoveNotification, null);
		}


		[Export ("tableViewStyleForCoder:")]
		static UITableViewStyle GetTableViewStyleForCoder (NSCoder decoder) => UITableViewStyle.Plain;


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			LeftButton.SetImage (UIImage.FromBundle ("i_image"), UIControlState.Normal);

			if (NavigationController?.NavigationBar != null)
			{
				LeftButton.TintColor = NavigationController.NavigationBar.TintColor;
				RightButton.TintColor = NavigationController.NavigationBar.TintColor;
			}

			TableView.RegisterClassForCellReuse (typeof (MessageCell), MessageCell.MessageCellReuseId);
			TableView.RegisterClassForCellReuse (typeof (MessageCell), MessageCell.MessageHeaderCellReuseId);
			TableView.RegisterClassForCellReuse (typeof (MessageCell), MessageCell.AutoCompleteReuseId);
		}


		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			BotClient.Shared.ReadyStateChanged += handleBotClientReadyStateChanged;
			BotClient.Shared.MessagesCollectionChanged += handleBotClientMessagesChanged;
			BotClient.Shared.UserTypingMessageReceived += handleBotClientUserTypingMessageReceived;
			BotClient.Shared.ForceSendMessage += handleBotClientForceSendMessage;
		}


		public override void ViewDidDisappear (bool animated)
		{
			BotClient.Shared.ReadyStateChanged -= handleBotClientReadyStateChanged;
			BotClient.Shared.MessagesCollectionChanged -= handleBotClientMessagesChanged;
			BotClient.Shared.UserTypingMessageReceived -= handleBotClientUserTypingMessageReceived;
			BotClient.Shared.ForceSendMessage -= handleBotClientForceSendMessage;

			base.ViewDidDisappear (animated);
		}


		public override void ViewWillLayoutSubviews ()
		{
			base.ViewWillLayoutSubviews ();

			TableView.ContentInset = new UIEdgeInsets (0, 0, 0, 0);
		}

		#endregion


		#region Action Methods

		void togglePIPWindow (object s, EventArgs e)
		{
			if (pipWindow == null)
			{
				showPIPWindow ();
			}
			else
			{
				hidePIPWindow ();
			}
		}


		void showPIPWindow ()
		{
			var frame = new CGRect (View.Frame.Width - 60, 0, 50, 50)
			{
				Y = TextInputbar.Frame.GetMinY () - 60
			};

			pipWindow = new UIWindow (frame)
			{
				BackgroundColor = UIColor.Black,
				Hidden = false,
				Alpha = 0
			};

			pipWindow.Layer.CornerRadius = 10;
			pipWindow.Layer.MasksToBounds = true;

			UIApplication.SharedApplication.KeyWindow.AddSubview (pipWindow);

			UIView.Animate (0.25, () => pipWindow.Alpha = 1.0f);
		}


		void hidePIPWindow ()
		{
			UIView.AnimateNotify (0.3f, () => pipWindow.Alpha = 0, delegate (bool _)
			{
				pipWindow.Hidden = true;
				pipWindow = null;
			});
		}


		[Export ("textInputbarDidMove:")]
		void textInputbarDidMove (NSNotification note)
		{
			if (pipWindow == null) return;

			CGRect frame = pipWindow.Frame;

			var origin = note.UserInfo.ObjectForKey (new NSString (@"origin")) as NSValue;

			frame.Y = origin.CGPointValue.Y - 60;

			pipWindow.Frame = frame;
		}

		#endregion


		#region Overriden Methods (Slack)

		public override bool CanPressRightButton => BotClient.Shared.Initialized;


		public override string KeyForTextCaching => NSBundle.MainBundle.BundleIdentifier;


		public override void TextDidUpdate (bool animated)
		{
			BotClient.Shared.SendUserTyping ();

			base.TextDidUpdate (animated);
		}


		// Notifies the view controller when the right button's action has been triggered, 
		//   manually
		public override void DidPressLeftButton (NSObject sender)
		{
			//base.DidPressLeftButton (sender);

			Log.Debug ("Left Button Pressed");

			showUploadActionSheet ();
		}


		// Notifies the view controller when the right button's action has been triggered, 
		//   manually or by using the keyboard return key.
		public override void DidPressRightButton (NSObject sender)
		{
			addNewMessage ();

			base.DidPressRightButton (sender);
		}


		void addNewMessage (bool send = true)
		{
			Log.Debug ($"addNewMessage {send}");

			// This little trick validates any pending auto-correction or auto-spelling just after hitting the 'Send' button
			TextView.RefreshFirstResponder ();

			var indexPath = NSIndexPath.FromRowSection (0, 0);
			var rowAnimation = Inverted ? UITableViewRowAnimation.Bottom : UITableViewRowAnimation.Top;
			var scrollPosition = Inverted ? UITableViewScrollPosition.Bottom : UITableViewScrollPosition.Top;

			TableView.BeginUpdates ();

			if ((send && BotClient.Shared.SendMessage (TextView.Text)) || !send)
			{
				TableView.InsertRows (new [] { NSIndexPath.FromRowSection (0, 0) }, rowAnimation);
			}

			TableView.EndUpdates ();

			var message = Messages.FirstOrDefault ();

			if (message != null)
			{
				TypingIndicatorView.RemoveUsername (message.Activity.From.Name);

				TableView.ScrollToRow (indexPath, scrollPosition, true);
			}

			// Fixes the cell from blinking (because of the transform, when using translucent cells)
			// See https://github.com/slackhq/SlackTextViewController/issues/94#issuecomment-69929927
			// TableView.ReloadRows (new [] { indexPath }, UITableViewRowAnimation.Automatic);
		}

		#endregion


		#region UITableViewSource Methods

		[Export ("numberOfSectionsInTableView:")]
		public nint NumberOfSections (UITableView tableView) => 1;


		public override nint RowsInSection (UITableView tableView, nint section)
			=> tableView.Equals (TableView) ? Messages?.Count ?? 0
											: searchResult?.Count ?? 0;


		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			=> tableView.Equals (TableView) ? Messages.GetMessageCell (tableView, indexPath)
											: searchResult.GetAutoCompleteCell (tableView, indexPath);


		[Export ("tableView:heightForRowAtIndexPath:")]
		public nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
			=> tableView.Equals (TableView) ? Messages.GetMessageHeight (indexPath)
											: MessageCell.AutoCompleteHeight;

		#endregion


		#region UITextViewDelegate Methods

		[Export ("textViewShouldBeginEditing:")]
		public bool ShouldBeginEditing (UITextView textView) => true;


		[Export ("textViewShouldEndEditing:")]
		public bool ShouldEndEditing (UITextView textView) => true;

		#endregion


		#region BotClient Event Handlers

		void handleBotClientForceSendMessage (object sender, string e)
		{
			Log.Debug ($"Force sending message: {e}");

			BeginInvokeOnMainThread (() =>
			{
				TextView.Text = e;
				addNewMessage ();
				TextView.Text = string.Empty;
			});
		}

		void handleBotClientMessagesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			Log.Debug ($"{e.Action}");

			BeginInvokeOnMainThread (() =>
			{
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:
						addNewMessage (false);
						break;
					case NotifyCollectionChangedAction.Remove:
						TableView.DeleteRows (new [] { NSIndexPath.FromIndex ((nuint)e.OldStartingIndex) }, UITableViewRowAnimation.None);
						break;
					case NotifyCollectionChangedAction.Replace:
						TableView.ReloadData ();
						break;
					case NotifyCollectionChangedAction.Reset:
						TableView.ReloadData ();
						TableView.SlackScrollToTop (false);
						break;
				}
			});
		}


		void handleBotClientReadyStateChanged (object sender, SocketStateChangedEventArgs e)
		{
			Log.Debug ($"{e.SocketState}");

			BeginInvokeOnMainThread (() =>
			{
				switch (e.SocketState)
				{
					case SocketStates.Open:
						//RightButton.Enabled = true;
						break;
					case SocketStates.Closing:
						//RightButton.Enabled = false;
						break;
				}
			});
		}


		void handleBotClientUserTypingMessageReceived (object sender, string e)
		{
			if (!string.IsNullOrEmpty (e))
			{
				TypingIndicatorView.InsertUsername (e);

				Task.Run (async () =>
				{
					await Task.Delay (3000);

					BeginInvokeOnMainThread (() => TypingIndicatorView.RemoveUsername (e));
				});
			}
		}

		#endregion


		#region Upload Image

		void showUploadActionSheet ()
		{
			var actionController = UIAlertController.Create ("Upload Image", null, UIAlertControllerStyle.ActionSheet);

			if (UIImagePickerController.IsSourceTypeAvailable (UIImagePickerControllerSourceType.Camera))
			{
				actionController.AddAction (UIAlertAction.Create ("Camera", UIAlertActionStyle.Default, handleUploadCameraAction));
			}

			actionController.AddAction (UIAlertAction.Create ("Photo Library", UIAlertActionStyle.Default, handleUploadPhotoLibraryAction));
			actionController.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, null));

			PresentViewController (actionController, true, null);
		}

		void handleUploadCameraAction (UIAlertAction obj) => showImagePickerForCamera ();

		void handleUploadPhotoLibraryAction (UIAlertAction obj) => showImagePicker (UIImagePickerControllerSourceType.PhotoLibrary);


		void showImagePickerForCamera ()
		{
			// Denies access to camera, alert the user.
			// The user has previously denied access. Remind the user that we need camera access to be useful.
			var authStatus = AVCaptureDevice.GetAuthorizationStatus (AVMediaType.Video);

			if (authStatus == AVAuthorizationStatus.Denied)
			{
				var alert = UIAlertController.Create ("Unable to access the Camera", "To enable access, go to Settings > Privacy > Camera and turn on Camera access for this app.", UIAlertControllerStyle.Alert);

				alert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, null));

				PresentViewController (alert, true, null);
			}
			else if (authStatus == AVAuthorizationStatus.NotDetermined)
			{
				// The user has not yet been presented with the option to grant access to the camera hardware.
				// Ask for it.
				AVCaptureDevice.RequestAccessForMediaType (AVMediaType.Video, (accessGranted) =>
				{
					// If access was denied, we do not set the setup error message since access was just denied.
					if (accessGranted)
					{
						// Allowed access to camera, go ahead and present the UIImagePickerController.
						showImagePicker (UIImagePickerControllerSourceType.Camera);
					}
				});
			}
			else
			{
				// Allowed access to camera, go ahead and present the UIImagePickerController.
				showImagePicker (UIImagePickerControllerSourceType.Camera);
			}
		}

		void showImagePicker (UIImagePickerControllerSourceType sourceType)
		{
			var imagePicker = new UIImagePickerController
			{
				Delegate = this,
				SourceType = sourceType
			};

			PresentViewController (imagePicker, true, null);
		}


		[Export ("imagePickerController:didFinishPickingMediaWithInfo:")]
		public void FinishedPickingMedia (UIImagePickerController picker, NSDictionary info)
		{
			DismissViewController (true, null);

			if (info.TryGetValue (UIImagePickerController.OriginalImage, out NSObject nsObject) && nsObject is UIImage image //&&
																															 /*info.TryGetValue (UIImagePickerController.ReferenceUrl, out NSObject nsReferenceUrl) && nsReferenceUrl is NSUrl referenceUrl*/)
			{
				Log.Debug ("Got Image!");

				Task.Run (async () =>
				{
					using (var stream = image.AsJPEG ().AsStream ())
					{
						if (await BotClient.Shared.SendUploadAsync (stream))
						{
							Log.Debug ("Image Upload Successful.");
						}
						else
						{
							Log.Debug ("Image Upload Failed.");
						}
					}
				});
			}
			else
			{
				Log.Debug ("Damnit!");
			}
		}

		[Export ("imagePickerControllerDidCancel:")]
		public void Canceled (UIImagePickerController picker)
		{
			DismissViewController (true, null);
		}

		#endregion

	}
}
