using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.DotNet;
using ApacheTech.VintageMods.Core.Services;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ILoadedSound"/> concrete implementation. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="SoundEffectsSettings" />
    public sealed class SoundEffectsPatches : WorldSettingsConsumer<SoundEffectsSettings>
    {
        /// <summary>
        ///     Manually patches the methods required to override individual game sound audio settings.
        /// </summary>
        /// <remarks>
        ///     This method will no longer be needed once the LoadedSound class has been de-obfuscated, within the vanilla API.
        /// </remarks>
        public static void ManualPatch()
        {
            // The concept here, is to cache the volume override settings for an individual sound, as a dynamic property,
            // when the sound is loaded. This will save reading from the settings file every time the sound is played, or
            // when the volume/pitch is changed.

            // There is currently no standardised way in which sounds are handled, and so this over-arching methodology must
            // be used, so we can account for all circumstances, and eventualities. The lookup table remains in memory as a 
            // singleton, and is cached here by reference; so any changes made to the dictionary entry will automatically
            // update when used here.

            var type = GameAssemblies.VintagestoryLib
                .GetTypes()
                .Where(p => p.IsClass)
                .FirstOrNull(p => typeof(ILoadedSound).IsAssignableFrom(p));

            Debug.Assert(type is not null, "Cannot find concrete implementation of ILoadedSound");

            var constructor = type.GetConstructor(new[] { typeof(ILogger), typeof(SoundParams), typeof(AudioMetaData) });

            var volumeMultiplier = type
                .GetRuntimeProperties()
                .Where(p => p.PropertyType == typeof(float))
                .Where(p => !p.CanWrite)
                .Where(p => p.Name != "SoundLengthSeconds")
                .Select(p => p.GetMethod)
                .FirstOrDefault();

            Debug.Assert(volumeMultiplier is not null, "Cannot find volume multiplier method.");

            const string methodName = nameof(Patch_ILoadedSound_Constructor_Postfix);
            var postfixMethod = typeof(SoundEffectsPatches).GetMethod(methodName);
            var harmonyMethod = new HarmonyMethod(postfixMethod);

            ModServices.Harmony.Default.Patch(constructor, postfix: harmonyMethod);

            ModServices.Harmony.Default.Patch(volumeMultiplier,
                postfix: new HarmonyMethod(
                    typeof(SoundEffectsPatches)
                        .GetMethod(nameof(Patch_ILoadedSound_VolumeMultiplier_Getter_Postfix))));
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the constructor of the <see cref="ILoadedSound"/> concrete class.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="ILoadedSound"/> this patch has been applied to.</param>
        public static void Patch_ILoadedSound_Constructor_Postfix(ILoadedSound __instance)
        {
            var path = __instance.Params.Location?.ToString();
            var volumeOverride = Settings.SoundAssets.FirstOrNull(p => p.Key == path);
            if (volumeOverride is null) return;
            __instance.Params.DynamicProperties().VolumeOverride = volumeOverride;
        }

        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "VolumeMultiplier" getter method in <see cref="ILoadedSound"/> concrete class.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="ILoadedSound"/> this patch has been applied to.</param>
        /// <param name="__result">The <see cref="float"/> value passed back from the original method.</param>
        public static void Patch_ILoadedSound_VolumeMultiplier_Getter_Postfix(ILoadedSound __instance, ref float __result)
        {
            if (Settings.MuteAll) __result = 0f;
            if (!DynamicEx.HasProperty(__instance.Params.DynamicProperties(), "VolumeOverride")) return;
            if (__instance.Params.DynamicProperties().VolumeOverride is not VolumeOverride volumeOverride) return;
            __result *= volumeOverride.Muted ? 0f : volumeOverride.VolumeMultiplier;
        }
    }
}
