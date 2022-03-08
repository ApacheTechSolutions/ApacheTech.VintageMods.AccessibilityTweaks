using ApacheTech.VintageMods.Core.Extensions.DotNet;
using HarmonyLib;
using Vintagestory.API.Client;

// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Patches
{
    public sealed partial class SoundEffectsPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "VolumeMultiplier" getter method in <see cref="ILoadedSound"/> concrete class.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="ILoadedSound"/> this patch has been applied to.</param>
        /// <param name="__result">The <see cref="float"/> value passed back from the original method.</param>
        public static void Patch_ILoadedSound_VolumeMultiplier_Getter_Postfix(ILoadedSound __instance, ref float __result)
        {
            if (Settings.MuteAll)
            {
                __result = 0f;
                return;
            }
            var path = __instance.Params.Location?.ToString();
            var volumeOverride = Settings.SoundAssets.FirstOrNull(p => p.Key == path);
            if (volumeOverride is null) return;
            __result *= volumeOverride.Muted ? 0f : volumeOverride.VolumeMultiplier;
        }
    }
}   