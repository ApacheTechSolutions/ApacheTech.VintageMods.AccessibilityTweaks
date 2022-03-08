using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks
{
    /// <summary>
    ///     - Feature: Visual Tweaks<br/><br/>
    ///
    ///     Various effects within the game can be harmful, and even potentially fatal to some players.<br/>
    ///     This mod helps to alleviate some of these issues by allowing the player to disable various rendered effects and sound effects.<br/><br/>
    ///
    ///         - Disable Raindrop Particles<br/>
    ///         - Disable Hailstone Particles<br/>
    ///         - Disable Snowflake Particles<br/>
    ///         - Disable Dust Particles<br/>
    ///         - Disable Lightning Flashes<br/>
    ///         - Disable Cloud Rendering<br/>
    ///         - Disable Fog Rendering<br/>
    ///         - Disable Camera Shaking (Shivering, etc.)<br/>
    ///         - Disable Temporal Storm Warp Effect
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class VisualTweaksProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterFeatureWorldSettings<VisualTweaksSettings>();

            services.RegisterTransient<Dialogue.VisualTweaksDialogue>();
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
            Patches.VisualTweaksPatches.Initialise();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Patches.VisualTweaksPatches.Dispose();
            base.Dispose();
        }
    }
}