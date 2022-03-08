using HarmonyLib;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ClientMain"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a Postfix patch to the "AddCameraShake" method in <see cref="ClientMain"/> class.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientMain), "AddCameraShake")]
        public static bool Patch_ClientMain_AddCameraShake_Prefix()
        {
            return Settings.CameraShakeEnabled;
        }
    }
}