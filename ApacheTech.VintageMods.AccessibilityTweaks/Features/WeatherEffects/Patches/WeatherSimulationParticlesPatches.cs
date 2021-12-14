using System;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable RedundantAssignment
// ReSharper disable StringLiteralTypo
// ReSharper disable IdentifierTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationParticlesPatches
    {
        private static readonly WeatherSettings Settings;
        private static WeatherSimulationParticles _instance;
        private static WeatherSystemClient _ws;
        
        static WeatherSimulationParticlesPatches()
        {
            Settings = ModServices.IOC.Resolve<WeatherSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "Initialize")]
        private static bool Patch_WeatherSimulationParticles_Initialize_Prefix(WeatherSimulationParticles __instance, ref Block ___lblock, ref WeatherSystemClient ___ws)
        {
            _instance = __instance;
            _ws = ___ws;
            ___lblock = ApiEx.Client.World.GetBlock(new AssetLocation("water-still-7"));
            if (___lblock != null)
            {
                ApiEx.Client.Event.RegisterAsyncParticleSpawner(AsyncParticleSpawn);
            }
            return false;
        }

        private static bool AsyncParticleSpawn(float dt, IAsyncParticleManager manager)
        {
            var data = _ws.BlendedWeatherData;
            var freezing = _ws.clientClimateCond?.Temperature < data.snowThresholdTemp;
            var state = data.BlendedPrecType switch
            {
                EnumPrecipitationType.Rain => Settings.RaindropsEnabled,
                EnumPrecipitationType.Snow => Settings.SnowEnabled,
                EnumPrecipitationType.Hail => Settings.HailEnabled,
                EnumPrecipitationType.Auto => freezing
                    ? Settings.SnowEnabled
                    : Settings.RaindropsEnabled,
                _ => throw new ArgumentOutOfRangeException()
            };
            if (state)
            {
               _instance.CallMethod<bool>("asyncParticleSpawn", dt, manager);
            }
            return true;
        }
    }
}