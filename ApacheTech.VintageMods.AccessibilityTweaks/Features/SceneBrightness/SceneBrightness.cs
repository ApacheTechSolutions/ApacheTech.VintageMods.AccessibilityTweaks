using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness
{
    /// <summary>
    ///  - Feature: Scene Brightness
    ///  
    ///     Helps people with light sensitivity, and for content creators, compensating for compression rates on YouTube / Twitch.
    ///  
    ///    - Adjust the brightness level of the game scene.
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class SceneBrightness : ClientModSystem
    {
        private SceneBrightnessSettings _settings;

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            _settings = ModServices.IOC.Resolve<SceneBrightnessSettings>();
            _settings.Enabled = false;

            FluentChat.ClientCommand("nv")
                .RegisterWith(api)
                .HasDescription(LangEx.FeatureString("SceneBrightness", "Description"))
                .HasDefaultHandler(ToggleSuperBright);
        }

        private void ToggleSuperBright(int groupId, CmdArgs args)
        {
            if (args.Length == 0)
            {
                _settings.Enabled = !_settings.Enabled;
                var enabledMessage = LangEx.FeatureString("SceneBrightness", "Enabled", LangEx.BooleanString(_settings.Enabled));
                ApiEx.Client.ShowChatMessage(enabledMessage);
                return;
            }
            _settings.Brightness = GameMath.Clamp(args.PopFloat().GetValueOrDefault(0.1f), 0f, 1f);
            var brightnessLevelMessage = LangEx.FeatureString("SceneBrightness", "BrightnessLevel", _settings.Brightness);
            ApiEx.Client.ShowChatMessage(brightnessLevelMessage);
        }
    }
}