using System;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class SystemRenderPlayerEffectsPatches
    {
        private static readonly WeatherSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="SystemRenderPlayerEffectsPatches"/> class.
        /// </summary>
        static SystemRenderPlayerEffectsPatches()
        {
            Settings = ModServices.IOC.Resolve<WeatherSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SystemRenderPlayerEffects), "onBeforeRender")]
        internal static bool Patch_SystemRenderPlayerEffects_onBeforeRender(
            ref SystemRenderPlayerEffects __instance, float dt, ref ClientMain ___game, ref int ___maxDynLights,
            ref NormalizedSimplexNoise ___noisegen, ref float ___curFreezingVal, ref long ___damangeVignettingUntil,
            ref int ___duration, float ___strength)
        {
            var shUniforms = ___game.GetField<DefaultShaderUniforms>("shUniforms");
            var mouseYaw = ___game.GetField<float>("mouseYaw");
            var mousePitch = ___game.GetField<float>("mousePitch");

            shUniforms.PointLightsCount = 0;
            var plrPos = ___game.EntityPlayer.Pos.XYZ;
            var array = ___game.GetEntitiesAround(plrPos, 60f, 60f, e => e.LightHsv != null && e.LightHsv[2] > 0);
            if (array.Length > ___maxDynLights)
            {
                array = (from e in array
                    orderby e.Pos.SquareDistanceTo(plrPos)
                    select e).ToArray();
            }
            foreach (var entity in array)
            {
                var lightHsv = entity.LightHsv;
                __instance.CallMethod("AddPointLight", lightHsv, entity.Pos);
            }

            if (ApiEx.Client.IsGamePaused) return false;
            var treeAttribute = ___game.EntityPlayer.WatchedAttributes.GetTreeAttribute("health");
            var num = treeAttribute.GetFloat("currenthealth") / treeAttribute.GetFloat("maxhealth");
            var num2 = Math.Max(0f, (0.23f - num) * 1f / 0.18f);
            var num3 = 0f;
            if (num2 > 0f)
            {
                var num4 = (float)(___game.InWorldEllapsedMs / 1000.0);
                var num5 = (float)___noisegen.Noise(12412.0, num4 / 2f) * 0.5f + (float)Math.Pow(Math.Abs(GameMath.Sin(num4 * 1f / 0.7f)), 30.0) * 0.5f;
                num3 = Math.Min(num2 * 1.5f, 1f) * (num5 * 0.75f + 0.5f);
                if (___game.EntityPlayer.Alive && Settings.CameraShakeEnabled)
                {
                    shUniforms.ExtraSepia = GameMath.Clamp(num2 * (float)___noisegen.Noise(0.0, num4 / 3f) * 1.2f, 0f, 1.2f);
                    if (___game.Rand.NextDouble() < 0.01)
                    {
                        ___game.AddCameraShake(0.15f * num2);
                    }
                    ___game.SetField("mouseYaw", mouseYaw + num2 * (float)(___noisegen.Noise(76.0, num4 / 50f) - 0.5) * 0.003f);
                    var num6 = num2 * (float)(___noisegen.Noise(num4 / 50f, 987.0) - 0.5) * 0.003f;
                    ___game.EntityPlayer.Pos.Pitch += num6;
                    ___game.SetField("mousePitch", mousePitch + num6);
                }
            }
            else
            {
                shUniforms.ExtraSepia = 0f;
            }
            var num7 = GameMath.Clamp((int)(___damangeVignettingUntil - ___game.ElapsedMilliseconds), 0, ___duration);
            shUniforms.DamageVignetting = GameMath.Clamp(GameMath.Clamp(___strength / 2f, 0.5f, 3.5f) * (num7 / (float)Math.Max(1, ___duration)) + num3, 0f, 1.5f);
            var @float = ___game.EntityPlayer.WatchedAttributes.GetFloat("freezingEffectStrength");
            ___curFreezingVal += (@float - ___curFreezingVal) * dt;
            if (___curFreezingVal > 0.1 && ApiEx.Client.World.Player.CameraMode == EnumCameraMode.FirstPerson && Settings.CameraShakeEnabled)
            {
                var num8 = (float)(___game.InWorldEllapsedMs / 1000.0);
                ___game.SetField("mouseYaw", mouseYaw + ClientSettings.CameraShakeStrength * (float)(Math.Max(0.0, ___noisegen.Noise(num8, 12.0) - 0.40000000596046448) * Math.Sin(num8 * 90f) * 0.01) * GameMath.Clamp(___curFreezingVal * 3f, 0f, 1f));
            }
            shUniforms.FrostVignetting = ___curFreezingVal;
            return false;
        }
    }
}