#if DEBUG
using System.Runtime.CompilerServices;
using System.Linq;
#endif

namespace NomadCode.BotFramework
{
	public static class Log
	{
#if DEBUG

		const string messageTypeInfo = " INFO";
		const string messageTypeDebug = " DEBUG";
		const string messageTypeError = " ERROR";


        public static void Debug (object caller, string memberName, string message)
        {
            System.Diagnostics.Debug.WriteLine (getPaddedString (messageTypeDebug, message, memberName, caller.GetType ().Name));
        }


        public static void Debug (string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine (getPaddedString (messageTypeDebug, message, memberName, sourceFilePath, sourceLineNumber));
        }


        public static void Info (string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine (getPaddedString (messageTypeInfo, message, memberName, sourceFilePath, sourceLineNumber));
        }
        

		public static void Error (string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine (getPaddedString (messageTypeError, message, memberName, sourceFilePath, sourceLineNumber));
        }


		static string getPaddedString (string messageType, string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
		{
			var sb = new System.Text.StringBuilder ($"[{System.DateTime.Now:MM/dd/yyyy h:mm:ss.fff tt}]");

			if (!string.IsNullOrWhiteSpace (sourceFilePath))
			{
				sourceFilePath = sourceFilePath.Split ('/').LastOrDefault ();

				sb.Append ($" [{sourceFilePath}]");
			}

			if (!string.IsNullOrWhiteSpace (memberName))
			{
				sb.Append ($" [{memberName}]");
			}

			if (sourceLineNumber > 0)
			{
				sb.Append ($" [{sourceLineNumber}]");
			}

			if (!string.IsNullOrEmpty (messageType))
			{
				sb.Append ($" {messageType} :: ");
			}

			if (!string.IsNullOrEmpty (message))
			{
				sb.Append (message);
			}

			return sb.ToString ();
		}
#else
		public static void Debug (string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { }
		public static void Info (string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { }
		public static void Error (string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0) { }
#endif

	}
}
