﻿using System;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public class WeatherSimulationParticlesPatches
    {
        private static readonly WeatherSettings Settings;

        /// <summary>
        /// 	Initialises static members of the <see cref="WeatherSimulationParticlesPatches"/> class.
        /// </summary>
        static WeatherSimulationParticlesPatches()
        {
            Settings = ModServices.IOC.Resolve<WeatherSettings>();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeatherSimulationParticles), "Initialize")]
        internal static bool Patch_WeatherSimulationParticles_Initialize_Prefix(ref WeatherSimulationParticles __instance)
        {
            _ws = __instance.GetField<WeatherSystemClient>("ws");
            _tmpPos = __instance.GetField<BlockPos>("tmpPos");
            _centerPos = __instance.GetField<BlockPos>("centerPos");
            _particlePos = __instance.GetField<Vec3d>("particlePos");
            _lowResRainHeightMap = __instance.GetField<int[,]>("lowResRainHeightMap");
            _parentVelocitySnow = __instance.GetField<Vec3f>("parentVeloSnow");
            _rainParticleColor = __instance.GetField<int>("rainParticleColor");

            _splashParticles = __instance.GetField<SimpleParticleProperties>("splashParticles");
            _dustParticles = __instance.GetField<SimpleParticleProperties>("dustParticles");
            _rainParticle = __instance.GetField<SimpleParticleProperties>("rainParticle");
            _hailParticle = __instance.GetField<SimpleParticleProperties>("hailParticle");
            _snowParticle = __instance.GetField<SimpleParticleProperties>("snowParticle");

            _lBlock = ApiEx.Client.World.GetBlock(new AssetLocation("water-still-7"));
            _rand = new Random(ApiEx.Client.World.Seed + 223123123);

            ApiEx.Client.Event.RegisterAsyncParticleSpawner(SpawnParticleAsync);
            return false;
        }

        private static bool SpawnParticleAsync(float dt, IAsyncParticleManager manager)
        {
            var weatherData = _ws.BlendedWeatherData;

            var conditions = _ws.clientClimateCond;
            if (conditions == null || !_ws.playerChunkLoaded) return true;

            EntityPos plrPos = ApiEx.Client.World.Player.Entity.Pos;
            var precipitationIntensity = conditions.Rainfall;

            var precipitationLevel = precipitationIntensity * ApiEx.Client.Settings.Int["particleLevel"] / 100f;

            _tmpPos.Set((int)plrPos.X, (int)plrPos.Y, (int)plrPos.Z);

            var precipitationType = weatherData.BlendedPrecType;
            if (precipitationType == EnumPrecipitationType.Auto)
                precipitationType = conditions.Temperature < weatherData.snowThresholdTemp
                    ? EnumPrecipitationType.Snow
                    : EnumPrecipitationType.Rain;

            _particlePos.Set(ApiEx.Client.World.Player.Entity.Pos.X, ApiEx.Client.World.Player.Entity.Pos.Y,
                ApiEx.Client.World.Player.Entity.Pos.Z);

            var onWaterSplashParticleColour = ApiEx.Client.World.ApplyColorMapOnRgba(_lBlock.ClimateColorMapForMap,
                _lBlock.SeasonColorMapForMap, ColorUtil.WhiteArgb, (int)_particlePos.X, (int)_particlePos.Y,
                (int)_particlePos.Z, false);
            var col = ColorUtil.ToBGRABytes(onWaterSplashParticleColour);
            onWaterSplashParticleColour = ColorUtil.ToRgba(94, col[0], col[1], col[2]);

            _centerPos.Set((int)_particlePos.X, 0, (int)_particlePos.Z);
            for (var lx = 0; lx < 16; lx++)
            {
                var dx = (lx - 8) * 4;
                for (var lz = 0; lz < 16; lz++)
                {
                    var dz = (lz - 8) * 4;

                    _lowResRainHeightMap[lx, lz] =
                        ApiEx.Client.World.BlockAccessor.GetRainMapHeightAt(_centerPos.X + dx, _centerPos.Z + dz);
                }
            }

            var rainYPos = ApiEx.Client.World.BlockAccessor.GetRainMapHeightAt((int)_particlePos.X, (int)_particlePos.Z);

            _parentVelocitySnow.X = -Math.Max(0, weatherData.curWindSpeed.X / 2 - 0.15f);
            _parentVelocitySnow.Y = 0;
            _parentVelocitySnow.Z = 0;

            if (weatherData.curWindSpeed.X > 0.7f && _particlePos.Y - rainYPos < 10)
            {
                var dx = (float)(plrPos.Motion.X * 40) - 50 * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _dustParticles.MinPos.Set(_particlePos.X - 40 + dx, _particlePos.Y + 15 + dy, _particlePos.Z - 40 + dz);
                _dustParticles.AddPos.Set(80, -20, 80);
                _dustParticles.GravityEffect = -0.1f - (float)_rand.NextDouble() * 0.1f;
                _dustParticles.ParticleModel = EnumParticleModel.Quad;
                _dustParticles.LifeLength = 1f;
                _dustParticles.DieOnRainHeightmap = true;
                _dustParticles.WindAffectednes = 8f;
                _dustParticles.MinQuantity = 0;
                _dustParticles.AddQuantity = 6 * (weatherData.curWindSpeed.X - 0.7f);

                _dustParticles.MinSize = 0.1f;
                _dustParticles.MaxSize = 0.4f;

                _dustParticles.MinVelocity.Set(-0.025f + 8 * weatherData.curWindSpeed.X, -0.2f, -0.025f);
                _dustParticles.AddVelocity.Set(0.05f + 4 * weatherData.curWindSpeed.X, 0.05f, 0.05f);


                for (var i = 0; i < 6; i++)
                {
                    var px = _particlePos.X + dx +
                             _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));
                    var pz = _particlePos.Z + dz +
                             _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));

                    var py = ApiEx.Client.World.BlockAccessor.GetRainMapHeightAt((int)px, (int)pz);
                    var block = ApiEx.Client.World.BlockAccessor.GetBlock((int)px, py, (int)pz);
                    if (block.IsLiquid()) continue;

                    _tmpPos.Set((int)px, py, (int)pz);
                    _dustParticles.Color = ColorUtil.ReverseColorBytes(block.GetColor(ApiEx.Client, _tmpPos));
                    _dustParticles.Color |= 255 << 24;

                    manager.Spawn(_dustParticles);
                }
            }

            if (precipitationIntensity <= 0.02) return true;

            if (Settings.HailEnabled && precipitationType == EnumPrecipitationType.Hail)
            {
                var dx = (float)(plrPos.Motion.X * 40) - 4 * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _hailParticle.MinPos.Set(_particlePos.X + dx, _particlePos.Y + 15 + dy, _particlePos.Z + dz);

                _hailParticle.MinSize = 0.3f * (0.5f + conditions.Rainfall);
                _hailParticle.MaxSize = 1f * (0.5f + conditions.Rainfall);

                _hailParticle.Color = ColorUtil.ToRgba(220, 210, 230, 255);

                _hailParticle.MinQuantity = 100 * precipitationLevel;
                _hailParticle.AddQuantity = 25 * precipitationLevel;
                _hailParticle.MinVelocity.Set(-0.025f + 7.5f * weatherData.curWindSpeed.X, -5f, -0.025f);
                _hailParticle.AddVelocity.Set(0.05f + 7.5f * weatherData.curWindSpeed.X, 0.05f, 0.05f);

                manager.Spawn(_hailParticle);
                return true;
            }

            if (Settings.RaindropsEnabled && precipitationType == EnumPrecipitationType.Rain)
            {
                var dx = (float)(plrPos.Motion.X * 80);
                var dy = (float)(plrPos.Motion.Y * 80);
                var dz = (float)(plrPos.Motion.Z * 80);

                _rainParticle.MinPos.Set(_particlePos.X - 30 + dx, _particlePos.Y + 15 + dy, _particlePos.Z - 30 + dz);
                _rainParticle.WithTerrainCollision = false;
                _rainParticle.MinQuantity = 1000 * precipitationLevel;
                _rainParticle.LifeLength = 1f;
                _rainParticle.AddQuantity = 25 * precipitationLevel;
                _rainParticle.MinSize = 0.15f * (0.5f + conditions.Rainfall);
                _rainParticle.MaxSize = 0.22f * (0.5f + conditions.Rainfall);
                _rainParticle.Color = _rainParticleColor;

                _rainParticle.MinVelocity.Set(-0.025f + 8 * weatherData.curWindSpeed.X, -10f, -0.025f);
                _rainParticle.AddVelocity.Set(0.05f + 8 * weatherData.curWindSpeed.X, 0.05f, 0.05f);

                manager.Spawn(_rainParticle);

                _splashParticles.MinVelocity = new Vec3f(-1f, 3, -1f);
                _splashParticles.AddVelocity = new Vec3f(2, 0, 2);
                _splashParticles.LifeLength = 0.1f;
                _splashParticles.MinSize = 0.07f * (0.5f + 0.65f * conditions.Rainfall);
                _splashParticles.MaxSize = 0.2f * (0.5f + 0.65f * conditions.Rainfall);
                _splashParticles.ShouldSwimOnLiquid = true;
                _splashParticles.Color = _rainParticleColor;

                var cnt = 100 * precipitationLevel;

                for (var i = 0; i < cnt; i++)
                {
                    var px = _particlePos.X + _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));
                    var pz = _particlePos.Z + _rand.NextDouble() * _rand.NextDouble() * 60 * (1 - 2 * _rand.Next(2));

                    var py = ApiEx.Client.World.BlockAccessor.GetRainMapHeightAt((int)px, (int)pz);

                    var block = ApiEx.Client.World.BlockAccessor.GetBlock((int)px, py, (int)pz);

                    if (block.IsLiquid())
                    {
                        _splashParticles.MinPos.Set(px, py + block.TopMiddlePos.Y - 1 / 8f, pz);
                        _splashParticles.AddVelocity.Y = 1.5f;
                        _splashParticles.LifeLength = 0.17f;
                        _splashParticles.Color = onWaterSplashParticleColour;
                    }
                    else
                    {
                        var b = 0.75 + 0.25 * _rand.NextDouble();
                        var ca = 230 - _rand.Next(100);
                        var cr = (int)(((_rainParticleColor >> 16) & 0xff) * b);
                        var cg = (int)(((_rainParticleColor >> 8) & 0xff) * b);
                        var cb = (int)(((_rainParticleColor >> 0) & 0xff) * b);

                        _splashParticles.Color = (ca << 24) | (cr << 16) | (cg << 8) | cb;
                        _splashParticles.AddVelocity.Y = 0f;
                        _splashParticles.LifeLength = 0.1f;
                        _splashParticles.MinPos.Set(px, py + block.TopMiddlePos.Y + 0.05, pz);
                    }

                    manager.Spawn(_splashParticles);
                }
            }

            if (Settings.SnowEnabled && precipitationType == EnumPrecipitationType.Snow)
            {
                var wetness = 2.5f * GameMath.Clamp(_ws.clientClimateCond.Temperature + 1, 0, 4) / 4f;

                var dx = (float)(plrPos.Motion.X * 40) - (30 - 9 * wetness) * weatherData.curWindSpeed.X;
                var dy = (float)(plrPos.Motion.Y * 40);
                var dz = (float)(plrPos.Motion.Z * 40);

                _snowParticle.MinVelocity.Set(-0.5f + 5 * weatherData.curWindSpeed.X, -1f, -0.5f);
                _snowParticle.AddVelocity.Set(1f + 5 * weatherData.curWindSpeed.X, 0.05f, 1f);
                _snowParticle.Color = ColorUtil.ToRgba(255, 255, 255, 255);

                _snowParticle.MinQuantity = 90 * precipitationLevel * (1 + wetness / 3);
                _snowParticle.AddQuantity = 15 * precipitationLevel * (1 + wetness / 3);
                _snowParticle.ParentVelocity = _parentVelocitySnow;
                _snowParticle.ShouldDieInLiquid = true;

                _snowParticle.LifeLength = Math.Max(1f, 4f - wetness);
                _snowParticle.Color = ColorUtil.ColorOverlay(ColorUtil.ToRgba(255, 255, 255, 255), _rainParticle.Color,
                    wetness / 4f);
                _snowParticle.GravityEffect = 0.005f * (1 + 20 * wetness);
                _snowParticle.MinSize = 0.1f * conditions.Rainfall;
                _snowParticle.MaxSize = 0.3f * conditions.Rainfall / (1 + wetness);

                const float hRange = 40f;
                const float vRange = 20f;
                _snowParticle.MinPos.Set(_particlePos.X - hRange + dx, _particlePos.Y + vRange + dy,
                    _particlePos.Z - hRange + dz);
                _snowParticle.AddPos.Set(2 * hRange + dx, -0.66f * vRange + dy, 2 * hRange + dz);

                manager.Spawn(_snowParticle);
            }

            return true;
        }

        private static Random _rand;

        private static WeatherSystemClient _ws;
        private static BlockPos _tmpPos = new();
        private static BlockPos _centerPos = new();
        private static Vec3d _particlePos = new();
        private static Block _lBlock;
        private static int[,] _lowResRainHeightMap = new int[16, 16];
        private static Vec3f _parentVelocitySnow = new();
        private static int _rainParticleColor;

        private static SimpleParticleProperties _splashParticles;
        private static SimpleParticleProperties _dustParticles;
        private static SimpleParticleProperties _rainParticle;
        private static SimpleParticleProperties _hailParticle;
        private static SimpleParticleProperties _snowParticle;
    }
}