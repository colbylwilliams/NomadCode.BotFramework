
namespace NomadCode.BotFramework
{
    using Newtonsoft.Json;
    using NomadCode.BotFramework;
    using System.Linq;

    /// <summary>
    /// Object of schema.org types
    /// </summary>
    public partial class Entity
    {
        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        public Entity()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Entity class.
        /// </summary>
        /// <param name="type">Entity Type (typically from schema.org
        /// types)</param>
        public Entity(string type = default(string))
        {
            Type = type;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets entity Type (typically from schema.org types)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}
