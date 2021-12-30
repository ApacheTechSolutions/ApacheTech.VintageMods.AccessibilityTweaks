using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationParticlesPatches : SettingsConsumer<RenderedEffectSettings>
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnHailParticles")]
        private static bool Patch_WeatherSimulationParticles_SpawnHailParticles_Prefix()
        {
            return Settings.HailstonesEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnRainParticles")]
        private static bool Patch_WeatherSimulationParticles_SpawnRainParticles_Prefix()
        {
            return Settings.RaindropsEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnSnowParticles")]
        private static bool Patch_WeatherSimulationParticles_SpawnSnowParticles_Prefix()
        {
            return Settings.SnowflakesEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "SpawnDustParticles")]
        private static bool Patch_WeatherSimulationParticles_SpawnDustParticles_Prefix()
        {
            return Settings.DustParticlesEnabled;
        }
    }
}