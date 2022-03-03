using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="WeatherSimulationLightning"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class WeatherSimulationLightningPatches : WorldSettingsConsumer<VisualTweaksSettings>
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "ClientTick" method in <see cref="WeatherSimulationLightning"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "ClientTick")]
        public static bool Patch_WeatherSimulationLightning_ClientTick_Prefix()
        {
            return Settings.LightningEnabled;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "OnRenderFrame" method in <see cref="WeatherSimulationLightning"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "OnRenderFrame")]
        public static bool Patch_WeatherSimulationLightning_OnRenderFrame_Prefix()
        {
            return Settings.LightningEnabled;
        }
    }
}