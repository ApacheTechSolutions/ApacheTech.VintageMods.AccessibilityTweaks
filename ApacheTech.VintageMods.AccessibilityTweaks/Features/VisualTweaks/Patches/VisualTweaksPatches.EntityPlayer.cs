using HarmonyLib;
using Vintagestory.API.Common;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="EntityPlayer"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "OnHurt" method in <see cref="EntityPlayer"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(EntityPlayer), "OnHurt")]
        public static bool Patch_EntityPlayer_OnHurt_Prefix()
        {
            return Settings.CameraShakeEnabled;
        }
    }
}