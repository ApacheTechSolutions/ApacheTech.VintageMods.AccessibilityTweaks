using System.Text;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects
{
    /// <summary>
    ///     Features: Weather Effects.
    /// 
    ///      • Toggle Rain Particle Effects
    ///      • Toggle Hail Particle Effects
    ///      • Toggle Snow Particle Effects
    ///      • Toggle Weather Sound Effects
    ///      • Toggle Fog Effects
    ///      • Toggle Cloud Render Effects
    ///      • Toggle Rain Particle Effects
    ///      • Toggle Lightning Lighting Effects
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class WeatherEffects : ClientModSystem
    {
        private WeatherSettings _settings;

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            _settings = ModServices.IOC.Resolve<WeatherSettings>();

            var sb = new StringBuilder(Lang.Get("accessibilitytweaks:weather-effects-settings-title"));
            var command = FluentChat.ClientCommand("wt")
                .RegisterWith(api)
                .HasDescription("accessibilitytweaks:weather-effects-command-description");

            foreach (var record in WeatherSettingsCommandMap.GetSettings())
            {
                sb.AppendLine(GetSettingMessage(record.Value));
                command.HasSubCommand(record.Key).WithHandler((_, _, _) => ToggleSetting(record.Value));
            }

            command.HasSubCommand("all").WithHandler((_, _, args) =>
            {
                var state = args.PopBool(true);
                foreach (var propertyName in WeatherSettingsCommandMap.GetSettings().Values)
                {
                    _settings.SetProperty(propertyName, state);
                }
                api.SendChatMessage(".wt settings");
            });

            command.HasSubCommand("settings").WithHandler((_, _, _) =>
            {
                api.SendChatMessage(".clearchat");
                api.ShowChatMessage(sb.ToString());
            });
        }

        private void ToggleSetting(string propertyName)
        {
            var value = _settings.GetProperty<bool>(propertyName);
            _settings.SetProperty(propertyName, !value);
            var message = GetSettingMessage(propertyName);
            ApiEx.Client.ShowChatMessage(message);
        }

        private string GetSettingMessage(string propertyName)
        {
            var value = _settings.GetProperty<bool>(propertyName);
            return Lang.Get(
                $"accessibilitytweaks:weather-effects-{propertyName}",
                Lang.Get($"vmods:boolean-value-{value}"));
        }
    }
}
