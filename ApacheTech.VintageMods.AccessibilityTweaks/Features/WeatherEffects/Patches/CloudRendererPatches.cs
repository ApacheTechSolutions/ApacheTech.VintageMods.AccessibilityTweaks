using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class CloudRendererPatches
    {
        private static readonly WeatherSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="CloudRendererPatches"/> class.
        /// </summary>
        static CloudRendererPatches()
        {
            Settings = ModServices.IOC.Resolve<WeatherSettings>();
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CloudRenderer), "CloudTick")]
        internal static bool Patch_CloudRenderer_CloudTick_Prefix()
        {
            return Settings.CloudsEnabled;
        }
    }
}