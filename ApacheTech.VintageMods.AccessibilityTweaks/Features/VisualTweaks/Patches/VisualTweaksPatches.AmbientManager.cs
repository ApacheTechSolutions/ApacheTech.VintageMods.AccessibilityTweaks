using System;
using HarmonyLib;
using Vintagestory.API.MathTools;
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
        ///     Applies a Postfix patch to the "UpdateAmbient" method in <see cref="AmbientManager"/>.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="AmbientManager"/> this patch has been applied to.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AmbientManager), "UpdateAmbient")]
        public static void Patch_AmbientManager_UpdateAmbient_Postfix(AmbientManager __instance)
        {
            const float minValue = 0.001f;
            if (!Settings.FogEnabled)
            {
                __instance.BlendedFogDensity = Math.Min(__instance.BlendedFogDensity, minValue);
                __instance.BlendedFogMin = Math.Min(__instance.BlendedFogMin, __instance.BlendedFogDensity);
            }

            if (Settings.MistEnabled) return;
            __instance.Base.FlatFogYPos = WeightedFloat.New(0, 0);
            __instance.Base.FlatFogDensity = WeightedFloat.New(0, 0);
            __instance.BlendedFlatFogYOffset = 0;
            __instance.BlendedFlatFogYPosForShader = -30f;
            __instance.BlendedFlatFogDensity = Math.Min(__instance.BlendedFlatFogDensity, minValue);
        }
    }
}