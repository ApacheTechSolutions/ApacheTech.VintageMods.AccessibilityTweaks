using ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection.Dialogue;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SceneBrightness.Dialogue;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Dialogue;
using ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Dialogue;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;
using Vintagestory.API.Config;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.AccessibilityHub.Dialogue
{
    /// <summary>
    ///     User interface that acts as a hub, to bring together all features within the mod.
    /// </summary>
    /// <seealso cref="GenericDialogue" />
    public sealed class AccessibilityHubDialogue : GenericDialogue
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="AccessibilityHubDialogue"/> class.
        /// </summary>
        /// <param name="capi">The client API.</param>
        public AccessibilityHubDialogue(ICoreClientAPI capi) : base(capi)
        {
            Alignment = EnumDialogArea.CenterMiddle;
            Title = LangEx.ModTitle();
            ModalTransparency = 0f;
            ShowTitleBar = true;
        }

        /// <summary>
        ///     Gets an entry from the language files, for the feature this instance is representing.
        /// </summary>
        /// <param name="code">The entry to return.</param>
        /// <returns>A localised <see cref="string"/>, for the specified language file code.</returns>
        private static string LangEntry(string code)
        {
            return LangEx.FeatureString("AccessibilityHub.Dialogue", code);
        }

        /// <summary>
        ///     Composes the header for the GUI.
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected override void ComposeBody(GuiComposer composer)
        {
            var row = 0f;
            const float width = 300f;
            const float heightOffset = 0f;

            composer.AddSmallButton(LangEntry("VisualEffects"), OnVisualEffectsButtonPressed, ButtonBounds(ref row, width, heightOffset));
            composer.AddSmallButton(LangEntry("SoundEffects"), OnSoundEffectsButtonPressed, ButtonBounds(ref row, width, heightOffset));
            composer.AddSmallButton(LangEntry("ColourCorrection"), OnColourCorrectionButtonPressed, ButtonBounds(ref row, width, heightOffset));
            composer.AddSmallButton(LangEntry("SceneBrightness"), OnSceneBrightnessButtonPressed, ButtonBounds(ref row, width, heightOffset));
            IncrementRow(ref row);
            composer.AddSmallButton(LangEx.GetCore("common-phrases.donate"), OnDonateButtonPressed, ButtonBounds(ref row, width, heightOffset));
            composer.AddSmallButton(Lang.Get("pause-back2game"), TryClose, ButtonBounds(ref row, width, heightOffset));
        }

        private static void IncrementRow(ref float row)
        {
            row += 0.5f;
        }

        private static ElementBounds ButtonBounds(ref float row, double width, double height)
        {
            IncrementRow(ref row);
            return ElementStdBounds
                .MenuButton(row, EnumDialogArea.LeftFixed)
                .WithFixedOffset(0, height)
                .WithFixedSize(width, 30);
        }

        private static bool OnVisualEffectsButtonPressed()
        {
            ModServices.IOC.Resolve<VisualTweaksDialogue>().Toggle();
            return true;
        }

        private static bool OnSoundEffectsButtonPressed()
        {
            ModServices.IOC.Resolve<SoundEffectsDialogue>().Toggle();
            return true;
        }

        private static bool OnColourCorrectionButtonPressed()
        {
            ModServices.IOC.Resolve<ColourCorrectionDialogue>().Toggle();
            return true;
        }

        private static bool OnSceneBrightnessButtonPressed()
        {
            ModServices.IOC.Resolve<SceneBrightnessDialogue>().Toggle();
            return true;
        }

        private static bool OnDonateButtonPressed()
        {
            CrossPlatform.OpenBrowser("https://bit.ly/APGDonate");
            return true;
        }
    }
}
