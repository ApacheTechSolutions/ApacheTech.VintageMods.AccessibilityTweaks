using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ClientMain"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class ClientMainPatches : WorldSettingsConsumer<VisualTweaksSettings>
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