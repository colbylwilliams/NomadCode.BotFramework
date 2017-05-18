using Android.Support.V7.Widget;

namespace NomadCode.BotFramework.Android
{
	public class MessageCellViewHolder : RecyclerView.ViewHolder
	{
		MessageCell _messageCell;

		public MessageCell MessageCell => _messageCell ?? (_messageCell = ItemView as MessageCell);


		public bool IsHead => ItemViewType > 0;


		public MessageCellViewHolder (MessageCell itemView) : base (itemView)
		{

		}
	}
}
