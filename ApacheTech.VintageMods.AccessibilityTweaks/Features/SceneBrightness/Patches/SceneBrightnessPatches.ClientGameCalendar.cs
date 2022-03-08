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
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "DayLightStrength" method in <see cref="ClientGameCalendar"/> class.
        /// </summary>
        /// <param name="__result">The <see cref="float"/> value passed into the original method.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientGameCalendar), "DayLightStrength", MethodType.Getter)]
        public static bool Patch_ClientGameCalendar_DayLightStrength_Prefix(ref float __result)
        {
            return ProcessValue(ref __result);
        }

        private static bool ProcessValue(ref float value)
        {
            if (!Settings.Enabled) return true;
            value = Math.Max(Settings.Brightness, value);
            return false;
        }
    }
}