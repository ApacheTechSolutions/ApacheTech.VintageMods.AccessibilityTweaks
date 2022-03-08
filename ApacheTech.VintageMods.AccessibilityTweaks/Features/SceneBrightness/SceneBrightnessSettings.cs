using ApacheTech.VintageMods.Core.Abstractions.Features;
using Newtonsoft.Json;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness
{
    /// <summary>
    ///     Contains all the settings that can be set by the `.superBright` chat command. This class cannot be inherited.
    /// </summary>
    [JsonObject]
    public sealed class SceneBrightnessSettings : FeatureSettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether SuperBright mode is enabled.
        /// </summary>
        /// <value><c>true</c> if SuperBright mode is enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets or sets the current brightness.
        /// </summary>
        /// <value>The current scene brightness.</value>
        public float Brightness { get; set; } = 0.1f;
    }
}