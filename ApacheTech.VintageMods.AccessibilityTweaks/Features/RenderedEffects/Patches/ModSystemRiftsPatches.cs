using System;
using System.Collections.Generic;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantAssignment

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class ModSystemRiftsPatches : WorldSettingsConsumer<RenderedEffectSettings>
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ModSystemRifts), "onClientTick")]
        private static void Patch_ModSystemRifts_onClientTick_Postfix(bool ___riftsEnabled, IReadOnlyList<Rift> ___nearestRifts, IReadOnlyList<ILoadedSound> ___riftSounds)
        {
            if (!___riftsEnabled) return;
            for (var i = 0; i < Math.Min(4, ___nearestRifts.Count); i++)
            {
                var rift = ___nearestRifts[i];
                var loadedSound = ___riftSounds[i];
                if (loadedSound.IsPlaying)
                {
                    loadedSound.SetVolume(GameMath.Clamp(rift.GetNowSize(ApiEx.Client) / 3f, 0.1f, 1f) * Settings.RiftVolume);
                }
            }
        }
    }
}