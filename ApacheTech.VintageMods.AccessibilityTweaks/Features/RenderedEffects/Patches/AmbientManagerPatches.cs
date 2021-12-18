using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0059 // Unnecessary assignment of a value

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class AmbientManagerPatches : FeaturePatch<RenderedEffectSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFlatFogDensity", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFlatFogDensity_Prefix(float __result)
        {
            __result = 1f;
            return Settings.FogEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFogMin", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFogMin_Prefix(float __result)
        {
            __result = 1f;
            return Settings.FogEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedFogBrightness", MethodType.Getter)]
        private static bool Patch_AmbientManager_BlendedFogBrightness_Prefix(float __result)
        {
            __result = 3f;
            return Settings.FogEnabled;
        }
    }
}