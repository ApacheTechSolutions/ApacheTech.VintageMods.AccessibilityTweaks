using System.Linq;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Patches;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.DotNet;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects
{
    /// <summary>
    ///     - Feature: Sound Effects<br/><br/>
    ///
    ///     The sounds within the game have not been normalised, so some sounds will be very loud, compared to others, even when played at the same game volume.<br/>
    ///     This feature allows the user to selectively choose the volume level for each in-game sound, individually, so that any white-noise can be filtered out.<br/>
    ///     Very useful for sufferers of Tinnitus, or white-noise affected epilepsy.<br/><br/>
    ///
    ///         - Change the volume of every sound in the game, individually.<br/>
    ///         - Game sounds mute, when the game is paused.<br/>
    ///         - Game sounds mute, when the game window loses focus.
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class SoundEffects : ClientModSystem
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="capi">The game's core client API.</param>
        public override void StartClientSide(ICoreClientAPI capi)
        {
            // Populate settings JSON file. 
            UpdateVolumeOverrideSettings();

            // Manually patch the obfuscated methods required to override sound file volumes.
            // NOTE: In a future game version, the LoadedSound class will be de-obfuscated.
            SoundEffectsPatches.ManualPatch();
        }

        private static void UpdateVolumeOverrideSettings()
        {
            var settings = ModServices.IOC.Resolve<SoundEffectsSettings>();
            var defaults = ApiEx.Client.Assets.AllAssets
                .Where(p => (p.Key.Category == AssetCategory.sounds || p.Key.Category == AssetCategory.music) && p.Key.Path.EndsWith(".ogg"));
            foreach (var entry in defaults)
            {
                var path = entry.Key.ToString();
                settings.SoundAssets.AddIfNotPresent(path, new VolumeOverride { Path = path });
            }
            ModSettings.World.Save(settings);
        }
    }
}