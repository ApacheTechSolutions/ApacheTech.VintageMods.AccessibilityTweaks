using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions;
using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="SystemTemporalStability"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        private static bool _previousStormActive;
        private static bool _shadersReloaded;

        /// <summary>
        ///     Prepares the class prior to patches, and cleans up the class when the patches are disposed.
        /// </summary>
        [HarmonyPrepare]
        [HarmonyCleanup]
        public static void Patch_SystemTemporalStability_Prepare_Cleanup()
        {
            _previousStormActive = false;
            _shadersReloaded = false;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "onServerData" method of the <see cref="SystemTemporalStability"/> class.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="SystemTemporalStability"/> this patch has been applied to.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SystemTemporalStability), "onServerData")]
        public static void Patch_SystemTemporalStability_onServerData_Postfix(SystemTemporalStability __instance)
        {
            var currentStormActive = __instance.StormData.nowStormActive;
            if (currentStormActive != _previousStormActive)
            {
                _previousStormActive = currentStormActive;
                _shadersReloaded = false;
            }
            if (_shadersReloaded) return;
            ApiEx.Client.Shader.ReloadShadersThreadSafe();
            _shadersReloaded = true;
        }
    }
}