using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="AmbientManager"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    public sealed partial class VisualTweaksPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "Update" method in <see cref="DefaultShaderUniforms"/>.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="AmbientManager"/> this patch has been applied to.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DefaultShaderUniforms), "Update")]
        public static bool Patch_DefaultShaderUniforms_Update_Prefix(DefaultShaderUniforms __instance)
        {
            if (Settings.MistEnabled) return true;
            __instance.FlagFogDensity = 0f;
            __instance.FlatFogStartYPos = 1024f;
            return true;
        }
    }
}