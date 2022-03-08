using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness
{
    /// <summary>
    ///  - Feature: Scene Brightness
    ///  
    ///     Helps people with light sensitivity, and for content creators, compensating for compression rates on YouTube / Twitch.
    ///  
    ///    - Adjust the brightness level of the game scene.
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class SceneBrightnessProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterFeatureWorldSettings<SceneBrightnessSettings>();
            services.RegisterSingleton<Dialogue.SceneBrightnessDialogue>();
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
            Patches.SceneBrightnessPatches.Initialise();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Patches.SceneBrightnessPatches.Dispose();
            base.Dispose();
        }
    }
}