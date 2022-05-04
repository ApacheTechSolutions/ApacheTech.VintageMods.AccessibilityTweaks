using System;
using HarmonyLib;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="AmbientManager"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "UpdateAmbient" method in <see cref="AmbientManager"/>.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="AmbientManager"/> this patch has been applied to.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AmbientManager), "UpdateAmbient")]
        public static void Patch_AmbientManager_UpdateAmbient_Postfix(AmbientManager __instance)
        {
            if (Settings.FogEnabled) return;
            __instance.BlendedFogDensity = Math.Min(__instance.BlendedFogDensity, 0.001f);
            __instance.BlendedFogMin = Math.Min(__instance.BlendedFogMin, __instance.BlendedFogDensity);
        }
    }
}