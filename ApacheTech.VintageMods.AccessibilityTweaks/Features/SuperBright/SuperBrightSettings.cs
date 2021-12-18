using Vintagestory.API.MathTools;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SuperBright
{
    /// <summary>
    ///     Contains all the settings that can be set by the `.superBright` chat command. This class cannot be inherited.
    /// </summary>
    public sealed class SuperBrightSettings
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
        public float Brightness { get; set; } = 1;

        /// <summary>
        ///     Gets or sets the colour of the ambient light.
        /// </summary>
        /// <value>The colour of the ambient light in the scene.</value>
        public Vec3f Colour { get; set; } = Vec3f.Zero;
    }
}