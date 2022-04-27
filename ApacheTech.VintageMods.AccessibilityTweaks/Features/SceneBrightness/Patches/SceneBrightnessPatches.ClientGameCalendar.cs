using System;
using HarmonyLib;
using Vintagestory.Common;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Patches
{
    public sealed partial class SceneBrightnessPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "DayLightStrength" method in the <see cref="ClientGameCalendar"/> class.
        /// </summary>
        /// <param name="__result">The <see cref="float"/> value passed into the original method.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientGameCalendar), "DayLightStrength", MethodType.Getter)]
        public static void Patch_ClientGameCalendar_DayLightStrength_Postfix(ref float __result)
        {
            if (!Settings.Enabled) return;
            __result = Math.Max(Settings.Brightness, __result);
        }
    }
}