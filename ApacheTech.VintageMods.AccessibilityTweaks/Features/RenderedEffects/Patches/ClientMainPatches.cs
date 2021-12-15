﻿using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class ClientMainPatches
    {
        private static readonly RenderedEffectSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="ClientMainPatches"/> class.
        /// </summary>
        static ClientMainPatches()
        {
            Settings = ModServices.IOC.Resolve<RenderedEffectSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ClientMain), "AddCameraShake")]
        private static bool Patch_ClientMain_AddCameraShake_Prefix()
        {
            return Settings.CameraShakeEnabled;
        }
    }
}