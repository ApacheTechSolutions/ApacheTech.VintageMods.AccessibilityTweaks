using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="WeatherSimulationParticles"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "SpawnHailParticles" method in <see cref="WeatherSimulationParticles"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnHailParticles")]
        public static bool Patch_WeatherSimulationParticles_SpawnHailParticles_Prefix()
        {
            return Settings.HailstonesEnabled;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "SpawnRainParticles" method in <see cref="WeatherSimulationParticles"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnRainParticles")]
        public static bool Patch_WeatherSimulationParticles_SpawnRainParticles_Prefix()
        {
            return Settings.RaindropsEnabled;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "SpawnSnowParticles" method in <see cref="WeatherSimulationParticles"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnSnowParticles")]
        public static bool Patch_WeatherSimulationParticles_SpawnSnowParticles_Prefix()
        {
            return Settings.SnowflakesEnabled;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "SpawnDustParticles" method in <see cref="WeatherSimulationParticles"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnDustParticles")]
        public static bool Patch_WeatherSimulationParticles_SpawnDustParticles_Prefix()
        {
            return Settings.DustParticlesEnabled;
        }
    }
}