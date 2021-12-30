using System.Reflection;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Dialogue
{
    /// <summary>
    ///     APL for RenderedEffects Feature.
    /// </summary>
    /// <seealso cref="FeatureSettingsDialogue{RenderedEffectSettings}" />
    public class RenderedEffectsDialogue : FeatureSettingsDialogue<RenderedEffectSettings>
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="RenderedEffectsDialogue"/> class.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="settings">The settings.</param>
        public RenderedEffectsDialogue(ICoreClientAPI capi, RenderedEffectSettings settings) 
            : base(capi, settings, "RenderedEffects")
        {
        }

        /// <summary>
        ///     The key combination string that toggles this GUI object.
        /// </summary>
        /// <value>The toggle key combination code.</value>
        public override string ToggleKeyCombinationCode => "renderedEffectsDialogue";

        protected override void ComposeBody(GuiComposer composer)
        {
            var leftColumnBounds = ElementBounds.Fixed(0, GuiStyle.TitleBarHeight + 1.0, 250, 20);
            var rightColumnBounds = ElementBounds.Fixed(260, GuiStyle.TitleBarHeight, 20, 20);
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                AddSettingSwitch(composer, propertyInfo.Name, ref leftColumnBounds, ref rightColumnBounds);
            }
        }

        private void AddSettingSwitch(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int switchSize = 20;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font.Clone().WithOrientation(EnumTextOrientation.Right), textBounds);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 260, textBounds);
            composer.AddSwitch(state => { Settings.SetProperty(propertyName, state); RefreshValues(); },
                sliderBounds.FlatCopy().WithFixedWidth(switchSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        protected override void RefreshValues()
        {
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                SingleComposer.GetSwitch($"btn{propertyInfo.Name}").SetValue((bool)propertyInfo.GetValue(Settings));
            }
            ClientSettings.WavingFoliage = Settings.GlitchEnabled;
            ApiEx.Client.Shader.ReloadShaders();
        }
    }
}
