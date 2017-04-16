using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector.DirectLine;
using Newtonsoft.Json;

namespace NomadCode.BotFramework
{
    // generic class to deserialize attachment content into
    public class AttachmentContent
    {
        // maybe set content type to know if the images are hero or thumbnail?

        [JsonProperty (PropertyName = "title")]
        public string Title { get; set; }

        public bool HasTitle => !string.IsNullOrEmpty (Title);

        /// <summary>
        /// Gets or sets subtitle of the card
        /// </summary>
        [JsonProperty (PropertyName = "subtitle")]
        public string Subtitle { get; set; }

        public bool HasSubtitle => !string.IsNullOrEmpty (Subtitle);

        /// <summary>
        /// Gets or sets text for the card
        /// </summary>
        [JsonProperty (PropertyName = "text")]
        public string Text { get; set; }

        public bool HasText => !string.IsNullOrEmpty (Text);

        /// <summary>
        /// Gets or sets array of images for the card
        /// </summary>
        [JsonProperty (PropertyName = "images")]
        public IList<CardImage> Images { get; set; }

        public bool HasImages => Images?.Count > 0;

        /// <summary>
        /// Gets or sets set of actions applicable to the current card
        /// </summary>
        [JsonProperty (PropertyName = "buttons")]
        public IList<CardAction> Buttons { get; set; }

        public bool HasButtons => Buttons?.Count > 0;

        /// <summary>
        /// Gets or sets this action will be activated when user taps on the
        /// card itself
        /// </summary>
        [JsonProperty (PropertyName = "tap")]
        public CardAction Tap { get; set; }
    }
}
