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
            var leftColumnBounds = ElementBounds.Fixed(0, GuiStyle.TitleBarHeight + 1.0, 200, 20);
            var rightColumnBounds = ElementBounds.Fixed(210, GuiStyle.TitleBarHeight, 20, 20);
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(bool))
                {
                    AddSettingSwitch(composer, propertyInfo.Name, ref leftColumnBounds, ref rightColumnBounds);
                }
                if (propertyInfo.PropertyType == typeof(float))
                {
                    AddSettingSlider(composer, propertyInfo.Name, ref leftColumnBounds, ref rightColumnBounds);
                }
            }
        }

        private bool OnRiftVolumeChanged(int value)
        {
            Settings.RiftVolume = value / 100f;
            return true;
        }

        private void AddSettingSwitch(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int switchSize = 20;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font, EnumTextOrientation.Left, textBounds.FlatCopy().WithFixedOffset(0,5));
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 260, textBounds);
            composer.AddSwitch(state => { Settings.SetProperty(propertyName, state); RefreshValues(); },
                sliderBounds.FlatCopy().WithFixedWidth(switchSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        private void AddSettingSlider(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int sliderSize = 100;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font, EnumTextOrientation.Left, textBounds);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 260, textBounds);
            composer.AddSlider(OnRiftVolumeChanged, sliderBounds.FlatCopy().WithFixedWidth(sliderSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        protected override void RefreshValues()
        {
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(bool))
                {
                    SingleComposer.GetSwitch($"btn{propertyInfo.Name}").SetValue((bool)propertyInfo.GetValue(Settings));
                }
                else if (propertyInfo.PropertyType == typeof(float))
                {
                    var rawValue = float.Parse(propertyInfo.GetValue(Settings).ToString());
                    var percentageValue = (int)(rawValue * 100);
                    SingleComposer.GetSlider($"btn{propertyInfo.Name}").SetValues(percentageValue, 0, 100, 1, "%");
                }
            }

            ClientSettings.WavingFoliage = Settings.GlitchEnabled;
            ApiEx.Client.Shader.ReloadShaders();
        }
    }
}
