
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Linq;

    /// <summary>
    /// A response containing a resource ID
    /// </summary>
    public partial class ResourceResponse
    {
        /// <summary>
        /// Initializes a new instance of the ResourceResponse class.
        /// </summary>
        public ResourceResponse()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ResourceResponse class.
        /// </summary>
        /// <param name="id">Id of the resource</param>
        public ResourceResponse(string id = default(string))
        {
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets id of the resource
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

    }
}
