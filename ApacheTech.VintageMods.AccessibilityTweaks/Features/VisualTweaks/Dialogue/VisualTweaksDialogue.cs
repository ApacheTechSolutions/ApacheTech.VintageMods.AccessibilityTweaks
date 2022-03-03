using System.Reflection;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Dialogue
{
    /// <summary>
    ///     APL for VisualTweaks Feature.
    /// </summary>
    /// <seealso cref="FeatureSettingsDialogue{TFeatureSettings}" />
    public sealed class VisualTweaksDialogue : FeatureSettingsDialogue<VisualTweaksSettings>
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="VisualTweaksDialogue"/> class.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="settings">The settings.</param>
        public VisualTweaksDialogue(ICoreClientAPI capi, VisualTweaksSettings settings) : base(capi, settings)
        {
            ModalTransparency = 0.0f;
            Alignment = EnumDialogArea.CenterMiddle;
        }

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
        protected override void RefreshValues()
        {
            if (!IsOpened()) return;
            foreach (var propertyInfo in Settings.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                SingleComposer
                    .GetSwitch($"btn{propertyInfo.Name}")
                    .SetValue((bool)propertyInfo.GetValue(Settings));
            }
            ClientSettings.RenderClouds = Settings.CloudsEnabled;
            ApiEx.Client.Shader.ReloadShadersThreadSafe();
        }

        /// <summary>
        ///     Composes the header for the GUI.
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected override void ComposeBody(GuiComposer composer)
        {
            const int switchSize = 20;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();

            var left = ElementBounds.Fixed(0, GuiStyle.TitleBarHeight + 1.0, 250, switchSize);
            var right = ElementBounds.Fixed(260, GuiStyle.TitleBarHeight, switchSize, switchSize);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.RaindropsEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.RaindropsEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnRaindropsEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.RaindropsEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.HailstonesEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.HailstonesEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnHailstonesEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.HailstonesEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.SnowflakesEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.SnowflakesEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnSnowflakesEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.SnowflakesEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.DustParticlesEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.DustParticlesEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnDustParticlesEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.DustParticlesEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.LightningEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.LightningEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnLightningEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.LightningEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.CloudsEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.CloudsEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnCloudsEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.CloudsEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.FogEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.FogEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnFogEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.FogEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.CameraShakeEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.CameraShakeEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnCameraShakeEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.CameraShakeEnabled)}");

            left = left.BelowCopy(fixedDeltaY: gapBetweenRows);
            right = right.BelowCopy(fixedDeltaY: gapBetweenRows);

            composer.AddStaticText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.GlitchEnabled)}"), font.Clone().WithOrientation(EnumTextOrientation.Right), left);
            composer.AddHoverText(LangEx.FeatureString(FeatureName, $"Dialogue.lbl{nameof(Settings.GlitchEnabled)}.HoverText"), font, 260, left);
            composer.AddSwitch(OnGlitchEnabledChanged, right.FlatCopy().WithFixedWidth(switchSize), $"btn{nameof(Settings.GlitchEnabled)}");
        }

        private void OnRaindropsEnabledChanged(bool state)
        {
            Settings.RaindropsEnabled = state;
            RefreshValues();
        }

        private void OnHailstonesEnabledChanged(bool state)
        {
            Settings.HailstonesEnabled = state;
            RefreshValues();
        }

        private void OnSnowflakesEnabledChanged(bool state)
        {
            Settings.SnowflakesEnabled = state;
            RefreshValues();
        }

        private void OnDustParticlesEnabledChanged(bool state)
        {
            Settings.DustParticlesEnabled = state;
            RefreshValues();
        }

        private void OnLightningEnabledChanged(bool state)
        {
            Settings.LightningEnabled = state;
            RefreshValues();
        }

        private void OnCloudsEnabledChanged(bool state)
        {
            Settings.CloudsEnabled = state;
            RefreshValues();
        }

        private void OnFogEnabledChanged(bool state)
        {
            Settings.FogEnabled = state;
            RefreshValues();
        }

        private void OnCameraShakeEnabledChanged(bool state)
        {
            Settings.CameraShakeEnabled = state;
            RefreshValues();
        }

        private void OnGlitchEnabledChanged(bool state)
        {
            Settings.GlitchEnabled = state;
            RefreshValues();
        }
    }
}
