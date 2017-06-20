
namespace NomadCode.BotFramework
{
	using Microsoft.Rest;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Conversations operations.
	/// </summary>
	public partial interface IConversations
	{
		/// <summary>
		/// Start a new conversation
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
		Task<HttpOperationResponse<Conversation>> StartConversationWithHttpMessagesAsync (Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
		/// <summary>
		/// Get information about an existing conversation
		/// </summary>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='watermark'>
		/// </param>
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
		/// <exception cref="Microsoft.Rest.ValidationException">
		/// Thrown when a required parameter is null
		/// </exception>
		Task<HttpOperationResponse<Conversation>> ReconnectToConversationWithHttpMessagesAsync (string conversationId, string watermark = default (string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
		/// <summary>
		/// Get activities in this conversation. This method is paged with the
		/// 'watermark' parameter.
		/// </summary>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='watermark'>
		/// (Optional) only returns activities newer than this watermark
		/// </param>
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
		/// <exception cref="Microsoft.Rest.ValidationException">
		/// Thrown when a required parameter is null
		/// </exception>
		Task<HttpOperationResponse<ActivitySet>> GetActivitiesWithHttpMessagesAsync (string conversationId, string watermark = default (string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
		/// <summary>
		/// Send an activity
		/// </summary>
		/// <param name='conversationId'>
		/// Conversation ID
		/// </param>
		/// <param name='activity'>
		/// Activity to send
		/// </param>
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
		/// <exception cref="Microsoft.Rest.ValidationException">
		/// Thrown when a required parameter is null
		/// </exception>
		Task<HttpOperationResponse<ResourceResponse>> PostActivityWithHttpMessagesAsync (string conversationId, Activity activity, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
		/// <summary>
		/// Upload file(s) and send as attachment(s)
		/// </summary>
		/// <param name='conversationId'>
		/// </param>
		/// <param name='file'>
		/// </param>
		/// <param name='userId'>
		/// </param>
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
		/// <exception cref="Microsoft.Rest.ValidationException">
		/// Thrown when a required parameter is null
		/// </exception>
		Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesAsync (string conversationId, Stream file, string userId = default (string), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));

		Task<HttpOperationResponse<ResourceResponse>> UploadWithHttpMessagesLitwareAsync (string conversationId, Stream file, string userId, Activity activity, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default (CancellationToken));
	}
}
