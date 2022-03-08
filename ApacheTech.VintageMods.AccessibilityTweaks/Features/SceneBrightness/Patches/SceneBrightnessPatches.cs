using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using Vintagestory.API.Common;
using Vintagestory.Common;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ClientGameCalendar"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="SceneBrightnessSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed partial class SceneBrightnessPatches : WorldSettingsConsumer<SceneBrightnessSettings>
    {
    }
}