using System.Reflection;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Dialogue
{
    /// <summary>
    ///     User interface for changing the settings for the Scene Brightness feature.
    /// </summary>
    /// <seealso cref="FeatureSettingsDialogue{SceneBrightnessSettings}" />
    public sealed class SceneBrightnessDialogue : FeatureSettingsDialogue<SceneBrightnessSettings>
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="SceneBrightnessDialogue"/> class.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="settings">The settings.</param>
        public SceneBrightnessDialogue(ICoreClientAPI capi, SceneBrightnessSettings settings) : base(capi, settings)
        {
            Alignment = EnumDialogArea.CenterMiddle;
            Modal = false;
        }

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
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
        }

        /// <summary>
        ///     Composes the header for the GUI.
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected override void ComposeBody(GuiComposer composer)
        {
            var leftColumnBounds = ElementBounds.Fixed(0, GuiStyle.TitleBarHeight + 1.0, 100, 20);
            var rightColumnBounds = ElementBounds.Fixed(110, GuiStyle.TitleBarHeight, 20, 20);
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

        /// <summary>
        ///     Gets whether it is preferred for the mouse to be not grabbed while this dialog is opened.
        ///     If true (default), the Alt button needs to be held to manually grab the mouse.
        /// </summary>
        /// <value><c>true</c> if the GUI prefers an un-grabbed mouse; otherwise, <c>false</c>.</value>
        public override bool PrefersUngrabbedMouse => true;

        private void AddSettingSwitch(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int switchSize = 20;
            const int gapBetweenRows = 30;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font, EnumTextOrientation.Left, textBounds.FlatCopy().WithFixedOffset(0, 5));
            composer.AddAutoSizeHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 160, textBounds);
            composer.AddSwitch(state => { Settings.SetProperty(propertyName, state); RefreshValues(); },
                sliderBounds.FlatCopy().WithFixedWidth(switchSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        private void AddSettingSlider(GuiComposer composer, string propertyName, ref ElementBounds textBounds, ref ElementBounds sliderBounds)
        {
            const int sliderSize = 200;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}"), font, EnumTextOrientation.Left, textBounds);
            composer.AddAutoSizeHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{propertyName}.HoverText"), font, 160, textBounds);
            composer.AddSlider(OnBrightnessChanged, sliderBounds.FlatCopy().WithFixedWidth(sliderSize), $"btn{propertyName}");
            textBounds = textBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
            sliderBounds = sliderBounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        private bool OnBrightnessChanged(int value)
        {
            Settings.Brightness = GameMath.Clamp(value / 100f, 0f, 1f);
            return true;
        }
    }
}
