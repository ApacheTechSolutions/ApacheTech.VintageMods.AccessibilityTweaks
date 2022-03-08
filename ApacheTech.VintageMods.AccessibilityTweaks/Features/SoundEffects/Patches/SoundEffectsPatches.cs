using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ILoadedSound"/> concrete implementation. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="SoundEffectsSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed partial class SoundEffectsPatches : WorldSettingsConsumer<SoundEffectsSettings>
    {
        /// <summary>
        ///     Manually patches the methods required to override individual game sound audio settings.
        /// </summary>
        /// <remarks>
        ///     This method will no longer be needed once the LoadedSound class has been de-obfuscated, within the vanilla API.
        /// </remarks>
        public static void ManualPatch()
        {
            // There is currently no standardised way in which sounds are handled, and so this over-arching methodology must
            // be used, so we can account for all circumstances, and eventualities. The lookup table remains in memory as a 
            // singleton, and is cached here by reference; so any changes made to the dictionary entry will automatically
            // update when used here.

            var type = GameAssemblies.VintagestoryLib
                .GetTypes()
                .Where(p => p.IsClass)
                .FirstOrNull(p => typeof(ILoadedSound).IsAssignableFrom(p));

            Debug.Assert(type is not null, "Cannot find concrete implementation of ILoadedSound");

            var volumeMultiplierMethod = type
                .GetRuntimeProperties()
                .Where(p => p.PropertyType == typeof(float))
                .Where(p => !p.CanWrite)
                .Where(p => p.Name != "SoundLengthSeconds")
                .Select(p => p.GetMethod)
                .FirstOrDefault();

            Debug.Assert(volumeMultiplierMethod is not null, "Cannot find volume multiplier method.");

            ModServices.Harmony.Default.Patch(volumeMultiplierMethod,
                postfix: new HarmonyMethod(
                    typeof(SoundEffectsPatches)
                        .GetMethod(nameof(Patch_ILoadedSound_VolumeMultiplier_Getter_Postfix))));
        }
    }
}
