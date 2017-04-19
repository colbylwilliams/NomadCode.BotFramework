
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Linq;

    /// <summary>
    /// An object representing a conversation or a conversation token
    /// </summary>
    public partial class Conversation
    {
        /// <summary>
        /// Initializes a new instance of the Conversation class.
        /// </summary>
        public Conversation()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Conversation class.
        /// </summary>
        /// <param name="conversationId">ID for this conversation</param>
        /// <param name="token">Token scoped to this conversation</param>
        /// <param name="expiresIn">Expiration for token</param>
        /// <param name="streamUrl">URL for this conversation's message
        /// stream</param>
        public Conversation(string conversationId = default(string), string token = default(string), int? expiresIn = default(int?), string streamUrl = default(string), string eTag = default(string))
        {
            ConversationId = conversationId;
            Token = token;
            ExpiresIn = expiresIn;
            StreamUrl = streamUrl;
            ETag = eTag;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets ID for this conversation
        /// </summary>
        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets token scoped to this conversation
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets expiration for token
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets URL for this conversation's message stream
        /// </summary>
        [JsonProperty(PropertyName = "streamUrl")]
        public string StreamUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }

    }
}
