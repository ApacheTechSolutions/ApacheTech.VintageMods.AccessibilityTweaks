using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class AmbientManagerPatches : WorldSettingsConsumer<RenderedEffectSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFlatFogDensity", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFlatFogDensity_Prefix(ref float __result)
        {
            if (!Settings.FogEnabled) __result = 0f;
            return Settings.FogEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFogMin", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFogMin_Prefix(ref float __result)
        {
            if (!Settings.FogEnabled) __result = 0f;
            return Settings.FogEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFogBrightness", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFogBrightness_Prefix(ref float __result)
        {
            if (!Settings.FogEnabled) __result = 0f;
            return Settings.FogEnabled;
        }
    }
}