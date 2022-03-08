using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects
{
    /// <summary>
    ///     - Feature: Sound Effects<br/><br/>
    ///
    ///     The sounds within the game have not been normalised, so some sounds will sounds very loud, compared to others, even when played at the same game volume.<br/>
    ///     This feature allows the user to selectively choose the volume level for each in-game sound, individually, so that any white-noise can be filtered out.<br/>
    ///     Very useful for sufferers of Tinnitus, or white-noise affected epilepsy.<br/><br/>
    ///
    ///         - Change the volume of every sound in the game, individually.<br/>
    ///         - Game sounds mute, when the game is paused.<br/>
    ///         - Game sounds mute, when the game window loses focus.
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class MusicLibraryProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterFeatureWorldSettings<SoundEffectsSettings>();
            services.RegisterTransient<Dialogue.SoundEffectsDialogue>();
        }

        /// <summary>
        ///     Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">
        ///     The core API implemented by the client.
        ///     The main interface for accessing the client.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        public override void StartPreClientSide(ICoreClientAPI capi)
        {
            Patches.SoundEffectsPatches.Initialise();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Patches.SoundEffectsPatches.Dispose();
            base.Dispose();
        }
    }
}