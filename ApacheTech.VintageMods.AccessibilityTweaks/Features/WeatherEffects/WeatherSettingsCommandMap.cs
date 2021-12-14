using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects
{
    /// <summary>
    ///     Contains a cross map of sub-command names, and their associated <see cref="WeatherSettings"/> property name. This class cannot be inherited.
    /// </summary>
    internal sealed class WeatherSettingsCommandMap : Dictionary<string, string>
    {
        /// <summary>
        ///     Gets a cross map of sub-command names, and their associated <see cref="WeatherSettings"/> property name.
        /// </summary>
        internal static Dictionary<string, string> GetSettings()
        {
            return new Dictionary<string, string>
            {
                { "lightning", "LightningEnabled" },
                { "rain", "RaindropsEnabled" },
                { "hail", "HailEnabled" },
                { "snow", "SnowEnabled" },
                { "fog", "FogEnabled" },
                { "clouds", "CloudsEnabled" },
                { "shake", "CameraShakeEnabled" },
                { "sounds", "SoundsEnabled" }
            };
        }
    }
}