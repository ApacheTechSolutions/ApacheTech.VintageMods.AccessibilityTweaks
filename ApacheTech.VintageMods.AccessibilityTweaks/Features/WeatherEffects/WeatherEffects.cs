using System.Collections.Generic;
using System.Text;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.WeatherEffects
{
    public class WeatherEffects : ClientModSystem
    {
        private WeatherSettings _settings;
        private Dictionary<string, string> _settingsMap;

        public override void StartClientSide(ICoreClientAPI api)
        {
            _settingsMap = new Dictionary<string, string>
            {
                { "lightning", "LightningEnabled" },
                { "rain", "RaindropsEnabled" },
                { "hail", "HailEnabled" },
                { "snow", "SnowEnabled" },
                { "fog", "FogEnabled" },
                { "clouds", "CloudsEnabled" },
                { "shake", "CameraShakeEnabled" },
                { "sounds", "SoundsEnabled" }
            };

            _settings = ModServices.IOC.Resolve<WeatherSettings>();
            var command = FluentChat.ClientCommand("wt", "weatherTweaks")
                .RegisterWith(api)
                .HasDescription("Provides Quality of Life tweaks for weather effects.")
                .HasSubCommand("settings").WithHandler((_, _, _) =>
                {
                    var sb = new StringBuilder();
                    sb.AppendLine(Lang.Get("accessibilitytweaks:weather-effects-settings-title"));
                    sb.AppendLine(GetSettingMessage("LightningEnabled"));
                    sb.AppendLine(GetSettingMessage("RaindropsEnabled"));
                    sb.AppendLine(GetSettingMessage("HailEnabled"));
                    sb.AppendLine(GetSettingMessage("SnowEnabled"));
                    sb.AppendLine(GetSettingMessage("FogEnabled"));
                    sb.AppendLine(GetSettingMessage("CloudsEnabled"));
                    sb.AppendLine(GetSettingMessage("CameraShakeEnabled"));
                    sb.AppendLine(GetSettingMessage("SoundsEnabled"));
                    api.SendChatMessage(".clearchat");
                    api.ShowChatMessage(sb.ToString());
                });

            foreach (var record in _settingsMap)
            {
                command
                    .HasSubCommand(record.Key)
                    .WithHandler((_, _, _) => ToggleSetting(record.Value));
            }
        }

        private void ToggleSetting(string propertyName)
        {
            var value = _settings.GetField<bool>(propertyName);
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
