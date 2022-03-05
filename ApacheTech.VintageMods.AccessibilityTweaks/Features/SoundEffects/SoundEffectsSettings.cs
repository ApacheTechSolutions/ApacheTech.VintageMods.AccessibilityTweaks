using System.Collections.Generic;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride;
using Newtonsoft.Json;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects
{
    /// <summary>
    ///     Represents the settings for the Sound Effects feature.
    /// </summary>
    [JsonObject]
    public sealed class SoundEffectsSettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether to temporarily mute all game sounds.
        /// </summary>
        /// <value><c>true</c> if all sounds should be muted; otherwise, <c>false</c>.</value>
        public bool MuteAll { get; set; }

        /// <summary>
        ///     Represents a list of all the sound files used by the game, including those added by mods.
        /// </summary>
        /// <value>A dictionary of sound files, with the asset path as the key.</value>
        public Dictionary<string, VolumeOverride> SoundAssets { get; set; } = new();
    }
}