
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Linq;

    /// <summary>
    /// Object describing a media thumbnail
    /// </summary>
    public partial class ThumbnailUrl
    {
        /// <summary>
        /// Initializes a new instance of the ThumbnailUrl class.
        /// </summary>
        public ThumbnailUrl()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ThumbnailUrl class.
        /// </summary>
        /// <param name="url">url pointing to an thumbnail to use for media
        /// content</param>
        /// <param name="alt">Alt text to display for screen readers on the
        /// thumbnail image</param>
        public ThumbnailUrl(string url = default(string), string alt = default(string))
        {
            Url = url;
            Alt = alt;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets url pointing to an thumbnail to use for media content
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets alt text to display for screen readers on the
        /// thumbnail image
        /// </summary>
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }

    }
}
