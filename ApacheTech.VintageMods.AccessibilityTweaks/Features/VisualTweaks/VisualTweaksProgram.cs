using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Dialogue;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Extensions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

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
            services.RegisterTransient<VisualTweaksDialogue>();
        }
    }
}