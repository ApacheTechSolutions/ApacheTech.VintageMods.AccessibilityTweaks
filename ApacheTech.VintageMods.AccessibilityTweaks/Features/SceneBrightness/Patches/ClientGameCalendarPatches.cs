using System;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Common;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ClientGameCalendar"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="SceneBrightnessSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class ClientGameCalendarPatches : WorldSettingsConsumer<SceneBrightnessSettings>
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "DayLightStrength" method in <see cref="ClientGameCalendar"/> class.
        /// </summary>
        /// <param name="value">The <see cref="float"/> value passed into the original method.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientGameCalendar), "DayLightStrength", MethodType.Setter)]
        public static void Patch_ClientGameCalendar_DayLightStrength_Prefix(ref float value)
        {
            if (!Settings.Enabled) return;
            value = Math.Max(Settings.Brightness, value);
        }
    }
}