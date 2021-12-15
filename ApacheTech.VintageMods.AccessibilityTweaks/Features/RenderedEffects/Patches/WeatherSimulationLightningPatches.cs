﻿using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationLightningPatches
    {
        private static readonly RenderedEffectSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="WeatherSimulationLightningPatches"/> class.
        /// </summary>
        static WeatherSimulationLightningPatches()
        {
            Settings = ModServices.IOC.Resolve<RenderedEffectSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "ClientTick")]
        private static bool Patch_WeatherSimulationLightning_ClientTick_Prefix()
        {
            return Settings.LightningEnabled;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationLightning), "OnRenderFrame")]
        private static bool Patch_WeatherSimulationLightning_OnRenderFrame_Prefix()
        {
            return Settings.LightningEnabled;
        }
    }
}