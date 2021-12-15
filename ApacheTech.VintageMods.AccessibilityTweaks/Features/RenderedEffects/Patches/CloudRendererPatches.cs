﻿using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class CloudRendererPatches
    {
        private static readonly RenderedEffectSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="CloudRendererPatches"/> class.
        /// </summary>
        static CloudRendererPatches()
        {
            Settings = ModServices.IOC.Resolve<RenderedEffectSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CloudRenderer), "CloudTick")]
        private static bool Patch_CloudRenderer_CloudTick_Prefix()
        {
            return Settings.CloudsEnabled;
        }
    }
}