using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable InconsistentNaming

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0060 // Remove unused parameter

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class ClientSettingsPatches : FeaturePatch<RenderedEffectSettings>
    {
        private static SystemTemporalStability _stabilitySystem;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientSettings), "WavingFoliage", MethodType.Getter)]
        private static bool Patch_ClientSettings_WavingFoliage_Prefix(bool __result)
        {
            _stabilitySystem ??= ApiEx.Client.ModLoader.GetModSystem<SystemTemporalStability>();
            if (!_stabilitySystem.StormData.nowStormActive) return true;
            __result = false;
            return Settings.GlitchEnabled;
        }
    }
}