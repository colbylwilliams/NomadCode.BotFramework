
namespace NomadCode.BotFramework
{
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Extension methods for Tokens.
	/// </summary>
	public static partial class TokensExtensions
	{
		/// <summary>
		/// Refresh a token
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		public static Conversation RefreshToken (this ITokens operations)
		{
			return operations.RefreshTokenAsync ().GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Refresh a token
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<Conversation> RefreshTokenAsync (this ITokens operations, CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.RefreshTokenWithHttpMessagesAsync (null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

		/// <summary>
		/// Generate a token for a new conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		public static Conversation GenerateTokenForNewConversation (this ITokens operations)
		{
			return operations.GenerateTokenForNewConversationAsync ().GetAwaiter ().GetResult ();
		}

		/// <summary>
		/// Generate a token for a new conversation
		/// </summary>
		/// <param name='operations'>
		/// The operations group for this extension method.
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		public static async Task<Conversation> GenerateTokenForNewConversationAsync (this ITokens operations, CancellationToken cancellationToken = default (CancellationToken))
		{
			using (var _result = await operations.GenerateTokenForNewConversationWithHttpMessagesAsync (null, cancellationToken).ConfigureAwait (false))
			{
				return _result.Body;
			}
		}

	}
}
