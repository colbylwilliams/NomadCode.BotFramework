
namespace NomadCode.BotFramework
{
	using Microsoft.Rest;
	using System.Collections;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Tokens operations.
	/// </summary>
	public partial interface ITokens
	{
		/// <summary>
		/// Refresh a token
		/// </summary>
		/// <param name='customHeaders'>
		/// The headers that will be added to request.
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		/// <exception cref="Microsoft.Rest.HttpOperationException">
		/// Thrown when the operation returned an invalid status code
		/// </exception>
		/// <exception cref="Microsoft.Rest.SerializationException">
		/// Thrown when unable to deserialize the response
		/// </exception>
		Task<HttpOperationResponse<Conversation>> RefreshTokenWithHttpMessagesAsync (Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
		/// <summary>
		/// Generate a token for a new conversation
		/// </summary>
		/// <param name='customHeaders'>
		/// The headers that will be added to request.
		/// </param>
		/// <param name='cancellationToken'>
		/// The cancellation token.
		/// </param>
		/// <exception cref="Microsoft.Rest.HttpOperationException">
		/// Thrown when the operation returned an invalid status code
		/// </exception>
		/// <exception cref="Microsoft.Rest.SerializationException">
		/// Thrown when unable to deserialize the response
		/// </exception>
		Task<HttpOperationResponse<Conversation>> GenerateTokenForNewConversationWithHttpMessagesAsync (Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
	}
}
