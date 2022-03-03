using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.AccessibilityHub.Dialogue;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.AccessibilityHub
{
    /// <summary>
    ///  - Feature: Accessibility Hub ***`(Default HotKey: F8)`***
    ///  
    ///     Provides easy access to all settings, within all features of the mod.
    ///  
    ///      - Visual Effects Settings
    ///      - Sound Effects Settings
    ///      - Colour Correction
    ///      - Scene Brightness
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class AccessibilityHubProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterTransient<AccessibilityHubDialogue>();
        }
    }
}
