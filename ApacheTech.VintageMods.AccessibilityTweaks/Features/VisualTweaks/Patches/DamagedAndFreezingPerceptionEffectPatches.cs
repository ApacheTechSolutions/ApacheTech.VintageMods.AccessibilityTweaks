using System;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="DamagedAndFreezingPerceptionEffect"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class DamagedAndFreezingPerceptionEffectPatches : WorldSettingsConsumer<VisualTweaksSettings>
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "OnBeforeGameRender" method of the <see cref="DamagedAndFreezingPerceptionEffect"/> class.
        /// </summary>
        /// <param name="dt">The delta time passed into original method.</param>
        /// <param name="___noisegen">The instance used by the original method.</param>
        /// <param name="___damangeVignettingUntil">The instance used by the original method.</param>
        /// <param name="___strength">The instance used by the original method.</param>
        /// <param name="___duration">The instance used by the original method.</param>
        /// <param name="___curFreezingVal">The instance used by the original method.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DamagedAndFreezingPerceptionEffect), "OnBeforeGameRender")]
        public static bool Patch_DamagedAndFreezingPerceptionEffect_OnBeforeGameRender_Prefix(float dt,
            NormalizedSimplexNoise ___noisegen, 
            long ___damangeVignettingUntil,
            float ___strength,
            int ___duration, 
            float ___curFreezingVal)
        {
            var capi = ApiEx.Client;
            if (capi.IsGamePaused) return false;
            var eplr = capi.World.Player;
            var healthTree = eplr.Entity.WatchedAttributes.GetTreeAttribute("health");
            var healthRel = healthTree == null ? 1 : healthTree.GetFloat("currenthealth") / healthTree.GetFloat("maxhealth");

            var f = Math.Max(0, (0.23f - healthRel) * 1 / 0.18f);
            float lowHealthness = 0;

            if (f > 0 && Settings.CameraShakeEnabled)
            {
                var ellapseSec = (float)(capi.InWorldEllapsedMilliseconds / 1000.0);

                var bla = (float)___noisegen.Noise(12412, ellapseSec / 2) * 0.5f + (float)Math.Pow(Math.Abs(GameMath.Sin(ellapseSec * 1 / 0.7f)), 30) * 0.5f;
                lowHealthness = Math.Min(f * 1.5f, 1) * (bla * 0.75f + 0.5f);

                if (eplr.Entity.Alive)
                {
                    capi.Render.ShaderUniforms.ExtraSepia = GameMath.Clamp(f * (float)___noisegen.Noise(0, ellapseSec / 3) * 1.2f, 0, 1.2f);
                    if (capi.World.Rand.NextDouble() < 0.01)
                    {
                        capi.World.AddCameraShake(0.15f * f);
                    }

                    capi.Input.MouseYaw += f * (float)(___noisegen.Noise(76, ellapseSec / 50) - 0.5f) * 0.003f;

                    var dp = f * (float)(___noisegen.Noise(ellapseSec / 50, 987) - 0.5f) * 0.003f;

                    eplr.Entity.Pos.Pitch += dp;
                    capi.Input.MousePitch += dp;
                }
            }
            else
            {
                capi.Render.ShaderUniforms.ExtraSepia = 0;
            }

            var val = GameMath.Clamp((int)(___damangeVignettingUntil - capi.ElapsedMilliseconds), 0, ___duration);

            capi.Render.ShaderUniforms.DamageVignetting = GameMath.Clamp(GameMath.Clamp(___strength / 2, 0.5f, 3.5f) * ((float)val / Math.Max(1, ___duration)) + lowHealthness, 0, 1.5f);

            if (Settings.CameraShakeEnabled)
            {
                var freezing = eplr.Entity.WatchedAttributes.GetFloat("freezingEffectStrength");

                ___curFreezingVal += (freezing - ___curFreezingVal) * dt;

                if (___curFreezingVal > 0.1 && eplr.CameraMode == EnumCameraMode.FirstPerson)
                {
                    var ellapseSec = (float)(capi.InWorldEllapsedMilliseconds / 1000.0);
                    capi.Input.MouseYaw += capi.Settings.Float["cameraShakeStrength"] * 
                                           (float)(Math.Max(0, ___noisegen.Noise(ellapseSec, 12) - 0.4f) * 
                                                   Math.Sin(ellapseSec * 90) * 0.01) * GameMath.Clamp(___curFreezingVal * 3, 0, 1);
                }
            }

            capi.Render.ShaderUniforms.FrostVignetting = ___curFreezingVal;
            return false;
        }
    }
}