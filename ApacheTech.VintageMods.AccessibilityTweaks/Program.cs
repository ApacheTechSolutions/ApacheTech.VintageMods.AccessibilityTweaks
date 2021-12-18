using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SuperBright;
using ApacheTech.VintageMods.Core.Hosting;
using ApacheTech.VintageMods.Core.Hosting.Configuration;
using ApacheTech.VintageMods.Core.Hosting.Configuration.Extensions;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks
{
    public class Program : ModHost
    {
        /// <summary>
        ///     Configures any services that need to be added to the IO Container, on the client side.
        /// </summary>
        /// <param name="services">The as-of-yet un-built services container.</param>
        protected override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterSingleton(_ => ModSettings.World.Feature<RenderedEffectSettings>("RenderedEffects"));
            services.RegisterSingleton(_ => ModSettings.World.Feature<SuperBrightSettings>("SuperBright"));
        }

        /// <summary>
        ///     Called on the client, during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="capi">
        ///     The core API implemented by the client.
        ///     The main interface for accessing the client.
        ///     Contains all sub-components, and some miscellaneous methods.
        /// </param>
        protected override void StartPreClientSide(ICoreClientAPI capi)
        {
            ModServices.FileSystem
                .RegisterSettingsFile("settings-world-client.json", FileScope.World);
        }

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The core API implemented by the client.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            ModServices.Harmony.UseHarmony();
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            FluentChat.DisposeClientCommands();
        }
    }
}
