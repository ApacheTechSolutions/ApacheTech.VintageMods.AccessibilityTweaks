using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class SystemTemporalStabilityPatches
    {
        private static readonly RenderedEffectSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="SystemTemporalStabilityPatches"/> class.
        /// </summary>
        static SystemTemporalStabilityPatches()
        {
            Settings = ModServices.IOC.Resolve<RenderedEffectSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SystemTemporalStability), "GetGlitchEffectExtraStrength")]
        private static bool Patch_SystemTemporalStability_GetGlitchEffectExtraStrength_Prefix(ref float __result)
        {
            __result = 0f;
            return Settings.GlitchEnabled;
        }
    }
}