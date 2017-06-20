using Android.Content;

using Android.Support.V7.Widget;

namespace NomadCode.BotFramework.Droid
{
	public class MessageCellViewHolder : RecyclerView.ViewHolder
	{
		MessageCell _messageCell;

		public MessageCell MessageCell => _messageCell ?? (_messageCell = ItemView as MessageCell);


		public bool IsHead => ItemViewType > MessageCell.HeadCellId;


		public MessageCellViewHolder (Context context, int viewType)
			: this (new MessageCell (context, viewType, new RecyclerView.LayoutParams (RecyclerView.LayoutParams.MatchParent, RecyclerView.LayoutParams.WrapContent)))
		{
		}

		public MessageCellViewHolder (MessageCell itemView)
			: base (itemView)
		{
		}
	}
}
