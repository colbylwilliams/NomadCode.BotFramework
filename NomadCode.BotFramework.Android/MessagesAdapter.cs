using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Content;
using Android.App;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Android.Widget;

namespace NomadCode.BotFramework.Droid
{
	public class MessagesAdapter : RecyclerView.Adapter
	{
		List<BotMessage> Messages => BotClient.Shared.Messages;

		MessagesView MessagesView;

		public MessagesAdapter (MessagesView messagesView)
		{
			MessagesView = messagesView;

			BotClient.Shared.ReadyStateChanged += handleBotClientReadyStateChanged;
			BotClient.Shared.MessagesCollectionChanged += handleBotClientMessagesChanged;
			BotClient.Shared.UserTypingMessageReceived += handleBotClientUserTypingMessageReceived;
		}


		void addNewMessage (bool send = true)
		{
			Log.Debug ($"addNewMessage {send}");

			//NotifyItemInserted (0);

			NotifyDataSetChanged ();

			// This little trick validates any pending auto-correction or auto-spelling just after hitting the 'Send' button
			//TextView.RefreshFirstResponder ();

			//var indexPath = NSIndexPath.FromRowSection (0, 0);
			//var rowAnimation = Inverted ? UITableViewRowAnimation.Bottom : UITableViewRowAnimation.Top;
			//var scrollPosition = Inverted ? UITableViewScrollPosition.Bottom : UITableViewScrollPosition.Top;

			//TableView.BeginUpdates ();

			//if ((send && BotClient.Shared.SendMessage (TextView.Text)) || !send)
			//{
			//	TableView.InsertRows (new [] { NSIndexPath.FromRowSection (0, 0) }, rowAnimation);
			//}

			//TableView.EndUpdates ();

			//var message = Messages.FirstOrDefault ();

			//if (message != null)
			//{
			//	TypingIndicatorView.RemoveUsername (message.Activity.From.Name);

			//	TableView.ScrollToRow (indexPath, scrollPosition, true);
			//}

			// Fixes the cell from blinking (because of the transform, when using translucent cells)
			// See https://github.com/slackhq/SlackTextViewController/issues/94#issuecomment-69929927
			// TableView.ReloadRows (new [] { indexPath }, UITableViewRowAnimation.Automatic);
		}


		#region RecyclerView.Adapter

		public override int ItemCount => Messages.Count;


		public override void OnBindViewHolder (RecyclerView.ViewHolder holder, int position)
		{
			if (holder is MessageCellViewHolder messageHolder)
			{
				Messages.SetMessageCell (messageHolder.MessageCell, position);
			}

			Log.Debug ($"OnBindViewHolder >> {position} : {holder}");
		}


		public override RecyclerView.ViewHolder OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			//var messageCell = new MessageCell (parent.Context, viewType, new RecyclerView.LayoutParams (RecyclerView.LayoutParams.MatchParent, RecyclerView.LayoutParams.WrapContent));

			//var holder = new MessageCellViewHolder (messageCell);
			var holder = new MessageCellViewHolder (parent.Context, viewType);

			Log.Debug ($"OnCreateViewHolder >> {viewType} : {holder}");

			return holder;
		}


		public override int GetItemViewType (int position)
		{
			var message = Messages [position];

			//HACK: fix this
			if (message.CellHeight < 1)
			{
				message.CellHeight = 2;

				message.Head = Messages.IsHead (position);
			}

			return Messages [position].Head ? MessageCell.HeadCellId : MessageCell.BodyCellId;
		}


		public override void OnViewRecycled (Java.Lang.Object holder)
		{
			if (holder is MessageCellViewHolder messageHolder)
			{
				messageHolder.MessageCell.PrepareForReuse ();
			}

			base.OnViewRecycled (holder);
		}


		#endregion


		#region BotClient Event Handlers

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
						NotifyItemRemoved (e.OldStartingIndex);
						//TableView.DeleteRows (new [] { NSIndexPath.FromIndex ((nuint)e.OldStartingIndex) }, UITableViewRowAnimation.None);
						break;
					case NotifyCollectionChangedAction.Replace:
						//NotifyItemChanged (e.OldStartingIndex);
						NotifyDataSetChanged ();
						//TableView.ReloadData ();
						break;
					case NotifyCollectionChangedAction.Reset:
						NotifyDataSetChanged ();
						//TableView.ReloadData ();
						//TableView.SlackScrollToTop (false);
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
				//TypingIndicatorView.InsertUsername (e);

				//Task.Run (async () =>
				//{
				//	await Task.Delay (3000);

				//	BeginInvokeOnMainThread (() => TypingIndicatorView.RemoveUsername (e));
				//});
			}
		}

		#endregion


		void BeginInvokeOnMainThread (Action action) => MessagesView?.Activity?.RunOnUiThread (action);
	}
}
