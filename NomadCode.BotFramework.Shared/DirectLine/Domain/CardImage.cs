
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Linq;

    /// <summary>
    /// An image on a card
    /// </summary>
    public partial class CardImage
    {
        /// <summary>
        /// Initializes a new instance of the CardImage class.
        /// </summary>
        public CardImage()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CardImage class.
        /// </summary>
        /// <param name="url">URL Thumbnail image for major content
        /// property.</param>
        /// <param name="alt">Image description intended for screen
        /// readers</param>
        /// <param name="tap">Action assigned to specific
        /// Attachment.E.g.navigate to specific URL or play/open media
        /// content</param>
        public CardImage(string url = default(string), string alt = default(string), CardAction tap = default(CardAction))
        {
            Url = url;
            Alt = alt;
            Tap = tap;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets URL Thumbnail image for major content property.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets image description intended for screen readers
        /// </summary>
        [JsonProperty(PropertyName = "alt")]
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets action assigned to specific Attachment.E.g.navigate to
        /// specific URL or play/open media content
        /// </summary>
        [JsonProperty(PropertyName = "tap")]
        public CardAction Tap { get; set; }

    }
}
