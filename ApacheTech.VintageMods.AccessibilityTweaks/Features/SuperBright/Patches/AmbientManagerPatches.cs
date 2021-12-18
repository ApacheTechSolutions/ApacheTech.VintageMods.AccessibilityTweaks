using System;
using System.Linq;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable InconsistentNaming

#pragma warning disable IDE0051

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SuperBright.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class AmbientManagerPatches : FeaturePatch<SuperBrightSettings>
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedSceneBrightness", MethodType.Getter)]
        private static void Patch_AmbientManager_BlendedSceneBrightness_Postfix(ref float __result)
        {
            if (!Settings.Enabled) return;
            var brightness = Math.Max(__result, Settings.Brightness);
            __result = brightness;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(AmbientManager), "BlendedAmbientColor", MethodType.Getter)]
        private static void Patch_AmbientManager_BlendedAmbientColor_Postfix(ref Vec3f __result)
        {
            if (!Settings.Enabled) return;
            __result ??= Vec3f.Zero;
            var colour = Settings.Colour;
            var colourBrightness = colour.ToDoubleArray().Sum();
            var resultBrightness = __result.ToDoubleArray().Sum();
            __result = (colourBrightness >= resultBrightness) ? colour : __result;
        }

    }
}