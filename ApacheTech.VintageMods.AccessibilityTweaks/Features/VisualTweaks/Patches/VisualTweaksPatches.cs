using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using Vintagestory.API.Common;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed partial class VisualTweaksPatches : WorldSettingsConsumer<VisualTweaksSettings>
    {

    }
}