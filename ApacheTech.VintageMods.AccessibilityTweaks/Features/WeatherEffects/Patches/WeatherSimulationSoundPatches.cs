using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationSoundPatches
    {
        private static readonly WeatherSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="WeatherSimulationSoundPatches"/> class.
        /// </summary>
        static WeatherSimulationSoundPatches()
        {
            Settings = ModServices.IOC.Resolve<WeatherSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationSound), "updateSounds")]
        internal static bool Patch_WeatherSimulationSound_updateSounds_Prefix()
        {
            return Settings.SoundsEnabled;
        }   
    }
}