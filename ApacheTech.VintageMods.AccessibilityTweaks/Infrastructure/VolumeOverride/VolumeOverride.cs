using Newtonsoft.Json;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride
{
    /// <summary>
    ///     Represents the overridden audio settings for an individual game sound file.
    /// </summary>
    [JsonObject]
    public sealed class VolumeOverride
    {
        /// <summary>
        ///     Gets or sets the asset path of the game sound file.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this individual game sound is muted.
        /// </summary>
        /// <value><c>true</c> if muted; otherwise, <c>false</c>.</value>
        public bool Muted { get; set; }

        /// <summary>
        ///     Gets or sets the volume multiplier to apply to an individual game sound.
        /// </summary>
        /// <value>A <see cref="float"/> value that represents the volume balance of an individual game sound.</value>
        public float VolumeMultiplier { get; set; } = 1f;

        /// <summary>
        ///     Gets or sets the pitch multiplier to apply to an individual game sound.
        /// </summary>
        /// <value>A <see cref="float"/> value that represents the pitch balance of an individual game sound.</value>
        public float PitchMultiplier { get; set; } = 1f;
    }
}