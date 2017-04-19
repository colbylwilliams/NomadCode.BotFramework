
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A collection of activities
    /// </summary>
    public partial class ActivitySet
    {
        /// <summary>
        /// Initializes a new instance of the ActivitySet class.
        /// </summary>
        public ActivitySet()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ActivitySet class.
        /// </summary>
        /// <param name="activities">Activities</param>
        /// <param name="watermark">Maximum watermark of activities within this
        /// set</param>
        public ActivitySet(IList<Activity> activities = default(IList<Activity>), string watermark = default(string))
        {
            Activities = activities;
            Watermark = watermark;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets activities
        /// </summary>
        [JsonProperty(PropertyName = "activities")]
        public IList<Activity> Activities { get; set; }

        /// <summary>
        /// Gets or sets maximum watermark of activities within this set
        /// </summary>
        [JsonProperty(PropertyName = "watermark")]
        public string Watermark { get; set; }

    }
}
