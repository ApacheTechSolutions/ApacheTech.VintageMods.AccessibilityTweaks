// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects
{
    public sealed class WeatherSettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether rain particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if rain particle effects are enabled; otherwise, <c>false</c>.</value>
        internal bool RaindropsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether lightning flashing light effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if lightning effects are enabled; otherwise, <c>false</c>.</value>
        internal bool LightningEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether snow particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if snow particle effects are enabled; otherwise, <c>false</c>.</value>
        internal bool SnowEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether hail particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if hail particle effects are enabled; otherwise, <c>false</c>.</value>
        internal bool HailEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether rain particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if rain particle effects are enabled; otherwise, <c>false</c>.</value>
        internal bool CloudsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether jittery shaking camera effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if jittery shaking camera effects are enabled; otherwise, <c>false</c>.</value>
        internal bool CameraShakeEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether weather sounds should be played, or not.
        /// </summary>
        /// <value><c>true</c> if weather sounds should be played; otherwise, <c>false</c>.</value>
        internal bool SoundsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether fog effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if fog effects are enabled; otherwise, <c>false</c>.</value>
        internal bool FogEnabled { get; set; }
    }
}