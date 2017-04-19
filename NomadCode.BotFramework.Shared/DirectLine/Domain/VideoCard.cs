
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A video card
    /// </summary>
    public partial class VideoCard
    {
        /// <summary>
        /// Initializes a new instance of the VideoCard class.
        /// </summary>
        public VideoCard()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the VideoCard class.
        /// </summary>
        /// <param name="aspect">Aspect ratio (16:9)(4:3)</param>
        /// <param name="title">Title of the card</param>
        /// <param name="subtitle">Subtitle of the card</param>
        /// <param name="text">Text of the card</param>
        /// <param name="image">Thumbnail placeholder</param>
        /// <param name="media">Array of media Url objects</param>
        /// <param name="buttons">Set of actions applicable to the current
        /// card</param>
        /// <param name="shareable">Is it OK for this content to be shareable
        /// with others (default:true)</param>
        /// <param name="autoloop">Should the client loop playback at end of
        /// content (default:true)</param>
        /// <param name="autostart">Should the client automatically start
        /// playback of video in this card (default:true)</param>
        public VideoCard(string aspect = default(string), string title = default(string), string subtitle = default(string), string text = default(string), ThumbnailUrl image = default(ThumbnailUrl), IList<MediaUrl> media = default(IList<MediaUrl>), IList<CardAction> buttons = default(IList<CardAction>), bool? shareable = default(bool?), bool? autoloop = default(bool?), bool? autostart = default(bool?))
        {
            Aspect = aspect;
            Title = title;
            Subtitle = subtitle;
            Text = text;
            Image = image;
            Media = media;
            Buttons = buttons;
            Shareable = shareable;
            Autoloop = autoloop;
            Autostart = autostart;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets aspect ratio (16:9)(4:3)
        /// </summary>
        [JsonProperty(PropertyName = "aspect")]
        public string Aspect { get; set; }

        /// <summary>
        /// Gets or sets title of the card
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets subtitle of the card
        /// </summary>
        [JsonProperty(PropertyName = "subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets text of the card
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets thumbnail placeholder
        /// </summary>
        [JsonProperty(PropertyName = "image")]
        public ThumbnailUrl Image { get; set; }

        /// <summary>
        /// Gets or sets array of media Url objects
        /// </summary>
        [JsonProperty(PropertyName = "media")]
        public IList<MediaUrl> Media { get; set; }

        /// <summary>
        /// Gets or sets set of actions applicable to the current card
        /// </summary>
        [JsonProperty(PropertyName = "buttons")]
        public IList<CardAction> Buttons { get; set; }

        /// <summary>
        /// Gets or sets is it OK for this content to be shareable with others
        /// (default:true)
        /// </summary>
        [JsonProperty(PropertyName = "shareable")]
        public bool? Shareable { get; set; }

        /// <summary>
        /// Gets or sets should the client loop playback at end of content
        /// (default:true)
        /// </summary>
        [JsonProperty(PropertyName = "autoloop")]
        public bool? Autoloop { get; set; }

        /// <summary>
        /// Gets or sets should the client automatically start playback of
        /// video in this card (default:true)
        /// </summary>
        [JsonProperty(PropertyName = "autostart")]
        public bool? Autostart { get; set; }

    }
}
