using System;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

#pragma warning disable IDE0051 // Remove unused private members

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherDataSnapshotPatches
    {
        private static readonly WeatherSettings _settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="WeatherDataSnapshotPatches"/> class.
        /// </summary>
        static WeatherDataSnapshotPatches()
        {
            _settings = ModServices.IOC.Resolve<WeatherSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherDataSnapshot), "SetAmbientLerped")]
        private static bool Patch_WeatherDataSnapshot_SetAmbientLerped_Prefix(ref WeatherDataSnapshot __instance, WeatherPattern left, WeatherPattern right, float w, float addFogDensity = 0f)
        {
            if (!_settings.FogEnabled && _settings.CloudsEnabled) return true;
            var num = GameMath.Clamp(1f - (float)Math.Pow(1.1 - __instance.climateCond.Rainfall, 4.0), 0f, 1f);

            __instance.Ambient.SceneBrightness.Value = left.State.nowSceneBrightness;
            __instance.Ambient.SceneBrightness.Weight = 1f;

            if (_settings.FogEnabled)
            {
                __instance.Ambient.FlatFogDensity.Value = (right.State.nowMistDensity * w + left.State.nowMistDensity * (1f - w)) / 250f;
                __instance.Ambient.FlatFogDensity.Weight = 1f;
                __instance.Ambient.FlatFogDensity.Weight *= num;

                __instance.Ambient.FlatFogYPos.Value = right.State.nowMistYPos * w + left.State.nowMistYPos * (1f - w);
                __instance.Ambient.FlatFogYPos.Weight = 1f;

                __instance.Ambient.FogDensity.Value = (addFogDensity + right.State.nowFogDensity * w + left.State.nowFogDensity * (1f - w)) / 1000f;
                __instance.Ambient.FogDensity.Weight = num;

                __instance.Ambient.FogBrightness.Value = right.State.nowFogBrightness * w + left.State.nowFogBrightness * (1f - w);
                __instance.Ambient.FogBrightness.Weight = 1f;
            }
            else
            {
                __instance.Ambient.FlatFogDensity.Value = 0f;
                __instance.Ambient.FlatFogDensity.Weight = 0f;

                __instance.Ambient.FlatFogYPos.Value = 0f;
                __instance.Ambient.FlatFogYPos.Weight = 0f;

                __instance.Ambient.FogDensity.Value = 0f;
                __instance.Ambient.FogDensity.Weight = 0f;

                __instance.Ambient.FogBrightness.Value = 0f;
                __instance.Ambient.FogBrightness.Weight = 0f;
            }

            if (!_settings.CloudsEnabled)
            {
                __instance.Ambient.CloudBrightness.Value = 0f;
                __instance.Ambient.CloudBrightness.Weight = 0f;

                __instance.Ambient.CloudDensity.Value = 0f;
                __instance.Ambient.CloudDensity.Weight = 0f;
            }
            else
            {
                __instance.Ambient.CloudBrightness.Value = right.State.nowCloudBrightness * w + left.State.nowCloudBrightness * (1f - w);
                __instance.Ambient.CloudBrightness.Weight = 1f;

                __instance.Ambient.CloudDensity.Value = right.State.nowbaseThickness * w + left.State.nowbaseThickness * (1f - w);
                __instance.Ambient.CloudDensity.Weight = 1f;
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherDataSnapshot), "SetAmbient")]
        internal static bool Patch_WeatherDataSnapshot_SetAmbient_Prefix(ref WeatherDataSnapshot __instance, WeatherPattern left, float addFogDensity = 0f)
        {
            if (!_settings.FogEnabled) return true;
            var num = GameMath.Clamp(1f - (float)Math.Pow(1.1 - __instance.climateCond.Rainfall, 4.0), 0f, 1f);

            __instance.Ambient.FlatFogDensity.Value = left.State.nowMistDensity / 250f;
            __instance.Ambient.FlatFogDensity.Weight = 1f;
            __instance.Ambient.FlatFogDensity.Weight *= num;

            __instance.Ambient.FlatFogYPos.Value = left.State.nowMistYPos;
            __instance.Ambient.FlatFogYPos.Weight = 1f;

            __instance.Ambient.FogDensity.Value = (addFogDensity + left.State.nowFogDensity) / 1000f;
            __instance.Ambient.FogDensity.Weight = num;

            __instance.Ambient.FogBrightness.Value = left.State.nowFogBrightness;
            __instance.Ambient.FogBrightness.Weight = 1f;

            __instance.Ambient.CloudBrightness.Value = left.State.nowCloudBrightness;
            __instance.Ambient.CloudBrightness.Weight = 1f;

            __instance.Ambient.CloudDensity.Value = left.State.nowbaseThickness;
            __instance.Ambient.CloudDensity.Weight = 1f;

            __instance.Ambient.SceneBrightness.Value = left.State.nowSceneBrightness;
            __instance.Ambient.SceneBrightness.Weight = 1f;

            return false;
        }
    }
}