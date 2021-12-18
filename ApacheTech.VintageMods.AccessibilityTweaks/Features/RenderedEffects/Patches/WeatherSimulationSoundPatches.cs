using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationSoundPatches : FeaturePatch<RenderedEffectSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationSound), "updateSounds")]
        private static bool Patch_WeatherSimulationSound_updateSounds_Prefix()
        {
            return Settings.SoundsEnabled;
        }   
    }
}