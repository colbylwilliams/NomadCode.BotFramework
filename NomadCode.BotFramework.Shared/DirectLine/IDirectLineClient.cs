
namespace NomadCode.BotFramework
{
	using Microsoft.Rest;
	using Newtonsoft.Json;

	/// <summary>
	/// Direct Line 3.0
	/// </summary>
	public partial interface IDirectLineClient : System.IDisposable
	{
		/// <summary>
		/// The base URI of the service.
		/// </summary>
		System.Uri BaseUri { get; set; }

		/// <summary>
		/// Gets or sets json serialization settings.
		/// </summary>
		JsonSerializerSettings SerializationSettings { get; }

		/// <summary>
		/// Gets or sets json deserialization settings.
		/// </summary>
		JsonSerializerSettings DeserializationSettings { get; }

		/// <summary>
		/// Subscription credentials which uniquely identify client
		/// subscription.
		/// </summary>
		ServiceClientCredentials Credentials { get; }


		/// <summary>
		/// Gets the IConversations.
		/// </summary>
		IConversations Conversations { get; }

		/// <summary>
		/// Gets the ITokens.
		/// </summary>
		ITokens Tokens { get; }

	}
}
