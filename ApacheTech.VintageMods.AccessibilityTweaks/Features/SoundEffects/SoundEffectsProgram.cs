using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Dialogue;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

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
    public sealed class SoundEffectsProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterFeatureWorldSettings<SoundEffectsSettings>();
            services.RegisterTransient<SoundEffectsDialogue>();
        }
    }
}