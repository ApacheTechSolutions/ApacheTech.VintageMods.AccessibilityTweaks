using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SuperBright
{
    /// <summary>
    ///     Feature: Super Bright
    ///
    ///      • Toggle super bright mode.
    ///      • Set the brightness level.
    ///      • Set the ambient light colour.
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    internal sealed class SuperBright : ClientModSystem
    {
        private SuperBrightSettings _settings;

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            _settings = ModServices.IOC.Resolve<SuperBrightSettings>();

            FluentChat.ClientCommand("superbright")
                .RegisterWith(api)
                .HasDescription(LangEx.FeatureString("SuperBright", "Description"))
                .HasDefaultHandler(ToggleSuperBright)
                .HasSubCommand("brightness").WithHandler(SetMinimumBrightness)
                .HasSubCommand("colour").WithHandler(SetAmbientColour);
        }

        private static void GetBlendedSceneBrightness()
        {
            ApiEx.Client.ShowChatMessage(LangEx.FeatureString("SuperBright", "BrightnessLevel",
                ((AmbientManager)ApiEx.Client.Ambient).BlendedSceneBrightness));
        }

        private void ToggleSuperBright(int groupId, CmdArgs args)
        {
            _settings.Enabled = !_settings.Enabled;
            ApiEx.Client.ShowChatMessage(LangEx.FeatureString("SuperBright", "Enabled", 
                LangEx.BooleanString(_settings.Enabled)));
        }

        private void SetMinimumBrightness(string subCommandName, int groupId, CmdArgs args)
        {
            _settings.Brightness = args.PopFloat().GetValueOrDefault(1f);
            GetBlendedSceneBrightness();
        }

        private void SetAmbientColour(string subCommandName, int groupId, CmdArgs args)
        {
            var r = args.PopFloat().GetValueOrDefault(1f);
            var g = args.PopFloat().GetValueOrDefault(r);
            var b = args.PopFloat().GetValueOrDefault(r);
            _settings.Colour = new Vec3f(r, g, b);
        }
    }
}