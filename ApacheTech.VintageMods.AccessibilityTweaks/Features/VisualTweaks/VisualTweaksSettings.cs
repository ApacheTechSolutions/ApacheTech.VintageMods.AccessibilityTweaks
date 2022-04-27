// ReSharper disable UnusedAutoPropertyAccessor.Global

using ApacheTech.VintageMods.Core.Abstractions.Features;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks
{
    /// <summary>
    ///     Contains all the settings that can be set by the `.wt` chat command. This class cannot be inherited.
    /// </summary>
    public sealed class VisualTweaksSettings : FeatureSettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether rain particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if rain particle effects are enabled; otherwise, <c>false</c>.</value>
        public bool RaindropsEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether hail particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if hail particle effects are enabled; otherwise, <c>false</c>.</value>
        public bool HailstonesEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether snow particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if snow particle effects are enabled; otherwise, <c>false</c>.</value>
        public bool SnowflakesEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether dust particle effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if dust particles effects are enabled; otherwise, <c>false</c>.</value>
        public bool DustParticlesEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether lightning flashing light effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if lightning effects are enabled; otherwise, <c>false</c>.</value>
        public bool LightningEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether clouds should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if clouds should be rendered; otherwise, <c>false</c>.</value>
        public bool CloudsEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether fog effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if fog effects are enabled; otherwise, <c>false</c>.</value>
        public bool FogEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether mist effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if mist effects are enabled; otherwise, <c>false</c>.</value>
        public bool MistEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether jittery shaking camera effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if jittery shaking camera effects are enabled; otherwise, <c>false</c>.</value>
        public bool CameraShakeEnabled { get; set; } = true;

        /// <summary>
        ///     Gets or sets a value indicating whether jittery shaking camera effects should be rendered, or not.
        /// </summary>
        /// <value><c>true</c> if jittery shaking camera effects are enabled; otherwise, <c>false</c>.</value>
        public bool GlitchEnabled { get; set; } = true;
    }
}