using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Dialogue;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

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
            services.RegisterSingleton<SceneBrightnessDialogue>();
        }
    }
}