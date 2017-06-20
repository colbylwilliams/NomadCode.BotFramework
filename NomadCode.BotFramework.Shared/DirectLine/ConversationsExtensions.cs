
namespace NomadCode.BotFramework
{
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for Conversations.
	/// </summary>
	public static partial class ConversationsExtensions
	{
		/// <summary>
		/// Start a new conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		public static Conversation StartConversation (this IConversations operations)
		{
			return operations.StartConversationAsync ().GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Start a new conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<Conversation> StartConversationAsync (this IConversations operations, CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.StartConversationWithHttpMessagesAsync (null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		/// <summary>
		/// Get information about an existing conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='watermark'>
		/// </param>
		public static Conversation ReconnectToConversation (this IConversations operations, string conversationId, string watermark = default (string))
		{
			return operations.ReconnectToConversationAsync (conversationId, watermark).GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Get information about an existing conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='watermark'>
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<Conversation> ReconnectToConversationAsync (this IConversations operations, string conversationId, string watermark = default (string), CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.ReconnectToConversationWithHttpMessagesAsync (conversationId, watermark, null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		/// <summary>
		/// Get activities in this conversation. This method is paged with the
		/// 'watermark' parameter.
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='watermark'>
		/// (Optional) only returns activities newer than this watermark
		/// </param>
		public static ActivitySet GetActivities (this IConversations operations, string conversationId, string watermark = default (string))
		{
			return operations.GetActivitiesAsync (conversationId, watermark).GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Get activities in this conversation. This method is paged with the
		/// 'watermark' parameter.
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='watermark'>
		/// (Optional) only returns activities newer than this watermark
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<ActivitySet> GetActivitiesAsync (this IConversations operations, string conversationId, string watermark = default (string), CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.GetActivitiesWithHttpMessagesAsync (conversationId, watermark, null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		/// <summary>
		/// Send an activity
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='activity'>
		/// Activity to send
		/// </param>
		public static ResourceResponse PostActivity (this IConversations operations, string conversationId, Activity activity)
		{
			return operations.PostActivityAsync (conversationId, activity).GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Send an activity
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='activity'>
		/// Activity to send
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<ResourceResponse> PostActivityAsync (this IConversations operations, string conversationId, Activity activity, CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.PostActivityWithHttpMessagesAsync (conversationId, activity, null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		/// <summary>
		/// Upload file(s) and send as attachment(s)
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='file'>
		/// </param>
		/// <param name='userId'>
		/// </param>
		public static ResourceResponse Upload (this IConversations operations, string conversationId, Stream file, string userId = default (string))
		{
			return operations.UploadAsync (conversationId, file, userId).GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Upload file(s) and send as attachment(s)
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='file'>
		/// </param>
		/// <param name='userId'>
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<ResourceResponse> UploadAsync (this IConversations operations, string conversationId, Stream file, string userId = default (string), CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.UploadWithHttpMessagesAsync (conversationId, file, userId, null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		public static async Task<ResourceResponse> UploadLitwareAsync (this IConversations operations, string conversationId, Stream file, string userId, Activity activity, CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.UploadWithHttpMessagesLitwareAsync (conversationId, file, userId, activity, null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}
	}
}
