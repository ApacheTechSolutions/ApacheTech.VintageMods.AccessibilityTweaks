using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection.Dialogue;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection.Shaders;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection
{
    /// <summary>
    ///  - Feature: Colour Correction
    ///  
    ///     Helps people with poor vision, light sensitivity, or colour-blindness.Playing around with the colour settings can give everything a warm, or cool hue.
    ///  
    ///     - Choose from different presets to simulate different colour vision deficiencies.
    ///     - Adjust the colour balance, and saturation of the game scene.
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class ColourCorrectionProgram : ClientFeatureRegistrar
    {
        /// <summary>
        /// Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterFeatureWorldSettings<ColourCorrectionSettings>();

            services.RegisterSingleton<ColourCorrectionRenderer>();

            services.RegisterTransient<ColourCorrectionDialogue>();
        }
    }
}