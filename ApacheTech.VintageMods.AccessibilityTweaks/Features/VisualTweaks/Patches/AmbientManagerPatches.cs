using System;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="AmbientManager"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class AmbientManagerPatches : WorldSettingsConsumer<VisualTweaksSettings>
    {
        /// <summary>
        ///     Applies a Postfix patch to the "UpdateAmbient" method in <see cref="AmbientManager"/>.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="AmbientManager"/> this patch has been applied to.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AmbientManager), "UpdateAmbient")]
        public static void Patch_AmbientManager_UpdateAmbient_Postfix(AmbientManager __instance)
        {
            if (Settings.FogEnabled) return;
            const float minValue = 0.001f;
            __instance.BlendedFlatFogDensity = Math.Min(__instance.BlendedFlatFogDensity, minValue);
            __instance.BlendedFogDensity = Math.Min(__instance.BlendedFogDensity, minValue);
            __instance.BlendedFogMin = Math.Min(__instance.BlendedFogMin, __instance.BlendedFogDensity);
        }
    }
}