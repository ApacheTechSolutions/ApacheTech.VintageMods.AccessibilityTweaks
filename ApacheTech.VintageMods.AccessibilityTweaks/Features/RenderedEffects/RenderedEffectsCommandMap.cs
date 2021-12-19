using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects
{
    /// <summary>
    ///     Contains a cross map of sub-command names, and their associated <see cref="RenderedEffectSettings"/> property name. This class cannot be inherited.
    /// </summary>
    internal sealed class RenderedEffectsCommandMap : Dictionary<string, string>
    {
        /// <summary>
        ///     Gets a cross map of sub-command names, and their associated <see cref="RenderedEffectSettings"/> property name.
        /// </summary>
        internal static Dictionary<string, string> GetSettings()
        {
            return new Dictionary<string, string>
            {
                { "lightning", "LightningEnabled" },
                { "rain", "RaindropsEnabled" },
                { "hail", "HailstonesEnabled" },
                { "snow", "SnowflakesEnabled" },
                { "dust", "DustParticlesEnabled" },
                { "fog", "FogEnabled" },
                { "clouds", "CloudsEnabled" },
                { "shake", "CameraShakeEnabled" },
                { "sounds", "SoundsEnabled" }
            };
        }
    }
}