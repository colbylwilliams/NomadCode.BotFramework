
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
using Android.Support.V7.Widget;

namespace NomadCode.BotFramework.Droid
{
	public class MessagesView : RecyclerView
	{
		public Android.App.Activity Activity => Context as Android.App.Activity;


		MessagesAdapter messagesAdapter;

		public MessagesView (Context context) :
			base (context)
		{
			Initialize ();
		}

		public MessagesView (Context context, IAttributeSet attrs) :
			base (context, attrs)
		{
			Initialize ();
		}

		public MessagesView (Context context, IAttributeSet attrs, int defStyle) :
			base (context, attrs, defStyle)
		{
			Initialize ();
		}

		void Initialize ()
		{
			//Rotation = 180;

			LinearLayoutManager llm = new LinearLayoutManager (Context/*, LinearLayoutManager.Vertical, false*/)
			{
				Orientation = LinearLayoutManager.Vertical
			};

			SetLayoutManager (llm);

			messagesAdapter = new MessagesAdapter (this);

			base.SetAdapter (messagesAdapter);
		}
	}
}
