using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="CloudRenderer"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a Prefix patch to the "CloudTick" method in <see cref="CloudRenderer"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CloudRenderer), "CloudTick")]
        public static bool Patch_CloudRenderer_CloudTick_Prefix()
        {
            return Settings.CloudsEnabled;
        }
    }
}