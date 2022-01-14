using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class DamagedAndFreezingPerceptionEffectPatches : WorldSettingsConsumer<RenderedEffectSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DamagedAndFreezingPerceptionEffect), "OnBeforeGameRender")]
        private static bool Patch_DamagedAndFreezingPerceptionEffect_OnBeforeGameRender_Prefix()
        {
            return Settings.CameraShakeEnabled;
        }
    }
}