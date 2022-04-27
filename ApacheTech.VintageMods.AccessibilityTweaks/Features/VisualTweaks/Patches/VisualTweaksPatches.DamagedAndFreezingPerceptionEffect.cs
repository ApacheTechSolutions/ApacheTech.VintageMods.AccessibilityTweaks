using System;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
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
    public sealed partial class VisualTweaksPatches
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
            ref float ___curFreezingVal)
        {
            var capi = ApiEx.Client;
            if (capi.IsGamePaused) return false;
            var player = capi.World.Player;
            HandleDamageEffects(___noisegen, ref ___damangeVignettingUntil, ___strength, ___duration, player, capi);
            HandleFreezingEffects(dt, ___noisegen, ref ___curFreezingVal, player, capi);
            return false;
        }

        private static void HandleDamageEffects(NormalizedSimplexNoise noisegen, ref long damageVignettingUntil,
            float ___strength, int ___duration, IPlayer player, ICoreClientAPI capi)
        {
            var healthTree = player.Entity.WatchedAttributes.GetTreeAttribute("health");
            var healthRel = healthTree?.GetFloat("currenthealth", 1f) / healthTree?.GetFloat("maxhealth") ?? 1f;

            var f = Math.Max(0, (0.23f - healthRel) * 1 / 0.18f);
            float lowHealthness = 0;

            if (f > 0)
            {
                var ellapseSec = (float)(capi.InWorldEllapsedMilliseconds / 1000.0);

                var bla = (float)noisegen.Noise(12412, ellapseSec / 2) * 0.5f +
                          (float)Math.Pow(Math.Abs(GameMath.Sin(ellapseSec * 1 / 0.7f)), 30) * 0.5f;

                lowHealthness = Math.Min(f * 1.5f, 1) * (bla * 0.75f + 0.5f);

                if (player.Entity.Alive)
                {
                    capi.Render.ShaderUniforms.ExtraSepia =
                        GameMath.Clamp(f * (float)noisegen.Noise(0, ellapseSec / 3) * 1.2f, 0, 1.2f);

                    if (capi.World.Rand.NextDouble() < 0.01)
                    {
                        capi.World.AddCameraShake(0.15f * f);
                    }

                    if (Settings.CameraShakeEnabled)
                    {
                        capi.Input.MouseYaw += f * (float)(noisegen.Noise(76, ellapseSec / 50) - 0.5f) * 0.003f;
                        var dp = f * (float)(noisegen.Noise(ellapseSec / 50, 987) - 0.5f) * 0.003f;
                        player.Entity.Pos.Pitch += dp;
                        capi.Input.MousePitch += dp;
                    }
                }
            }
            else
            {
                capi.Render.ShaderUniforms.ExtraSepia = 0;
            }

            var duration = GameMath.Clamp((int)(damageVignettingUntil - capi.ElapsedMilliseconds), 0, ___duration);
            var strength = GameMath.Clamp(___strength / 2, 0.5f, 3.5f);
            var relativeHealth = (float)duration / Math.Max(1, ___duration) + lowHealthness;
            capi.Render.ShaderUniforms.DamageVignetting = GameMath.Clamp(strength * relativeHealth, 0, 1.5f);
        }

        private static void HandleFreezingEffects(float dt, NormalizedSimplexNoise noisegen, ref float currentFreezingValue,
            IClientPlayer player, ICoreClientAPI capi)
        {
            var freezing = player.Entity.WatchedAttributes.GetFloat("freezingEffectStrength");

            currentFreezingValue += (freezing - currentFreezingValue) * dt;
            capi.Render.ShaderUniforms.FrostVignetting = currentFreezingValue;

            if (!Settings.CameraShakeEnabled || 
                currentFreezingValue <= 0.1 || 
                player.CameraMode != EnumCameraMode.FirstPerson) return;

            var ellapsedSeconds = (float)(capi.InWorldEllapsedMilliseconds / 1000.0);
            capi.Input.MouseYaw += capi.Settings.Float["cameraShakeStrength"] *
                                   (float)(Math.Max(0, noisegen.Noise(ellapsedSeconds, 12) - 0.4f) *
                                           Math.Sin(ellapsedSeconds * 90) * 0.01) * GameMath.Clamp(currentFreezingValue * 3, 0, 1);

        }
    }
}