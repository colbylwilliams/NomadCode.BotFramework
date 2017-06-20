//using System;
//using System.Collections.Generic;
//using Android.Views;
//using Android.Widget;
//using NomadCode.BotFramework.Droid;

//namespace NomadCode.BotFramework
//{
//	public class MessagesBaseAdapter : BaseAdapter<BotMessage>
//	{
//		List<BotMessage> Messages => BotClient.Shared.Messages;

//		public MessagesBaseAdapter ()
//		{
//		}

//		#region BaseAdapter<BotMessage>

//		public override BotMessage this [int position] => Messages[position];

//		public override int Count => Messages.Count;

//		public override long GetItemId (int position)
//		{
//			return position;//Messages[position]
//		}

//		public override View GetView (int position, View convertView, ViewGroup parent)
//		{
//			//var view = parent.Context.infl
//		}

//		#endregion
//	}
//}
