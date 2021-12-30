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

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class ClientSettingsPatches : SettingsConsumer<RenderedEffectSettings>
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientSettings), "WavingFoliage", MethodType.Getter)]
        private static void Patch_ClientSettings_WavingFoliage_Postfix(ref bool __result)
        {
            var stabilitySystem = ApiEx.Client.ModLoader.GetModSystem<SystemTemporalStability>();
            if (!stabilitySystem.StormData.nowStormActive) return;
            __result = __result && Settings.GlitchEnabled;
        }
    }
}