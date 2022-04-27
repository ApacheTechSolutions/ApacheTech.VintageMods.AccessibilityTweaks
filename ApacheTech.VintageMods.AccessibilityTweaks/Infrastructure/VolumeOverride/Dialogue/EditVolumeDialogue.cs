using System;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride.Dialogue
{
    /// <summary>
    ///     User interface for changing the volume and pitch settings for an individual sound effect within the game.
    /// </summary>
    /// <seealso cref="GenericDialogue" />
    public sealed class EditVolumeDialogue : GenericDialogue
    {
        private readonly VolumeOverride _cell;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="EditVolumeDialogue"/> class.
        /// </summary>
        /// <param name="capi">The core client API.</param>
        /// <param name="cell">The cell from main sound effects dialogue.</param>
        public EditVolumeDialogue(ICoreClientAPI capi, VolumeOverride cell) : base(capi)
        {
            _cell = cell;
            ModalTransparency = 0.4f;
            Alignment = EnumDialogArea.CenterMiddle;
            Title = ApiEx.Client.Assets.Get(cell.Path).Name;
        }

        /// <summary>
        ///     Gets an entry from the language files, for the feature this instance is representing.
        /// </summary>
        /// <param name="code">The entry to return.</param>
        /// <returns>A localised <see cref="string"/>, for the specified language file code.</returns>
        private static string LangEntry(string code)
        {
            return LangEx.FeatureString("SoundEffects.Dialogue", code);
        }

        /// <summary>
        ///     The action to perform when the OK button is pressed.
        /// </summary>
        /// <value>The <see cref="VolumeOverride"/> instance, passed back from this window.</value>
        public Action<VolumeOverride> OnOkAction { get; set; }

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
        protected override void RefreshValues()
        {
            var volume = (int)(_cell.VolumeMultiplier * 100);
            SingleComposer.GetSlider("sldVolume").SetValues(volume, 0, 200, 1, "%");

            // NOTE: Can't implement Pitch Shifting, until the LoadedSound class is de-obfuscated.
            //var pitch = (int)(_cell.PitchMultiplier * 100);
            //SingleComposer.GetSlider("sldPitch").SetValues(pitch, 0, 200, 1, "%");

            SingleComposer.GetSwitch("btnMute").SetValue(_cell.Muted);
        }

        /// <summary>
        ///     Composes the GUI components for this instance.
        /// </summary>
        protected override void Compose()
        {
            base.Compose();
            SingleComposer.GetButton("btnPlaySound").PlaySound = false;
        }

        /// <summary>
        ///     Composes the header for the GUI.
        /// </summary>
        /// <param name="composer">The composer.</param>
        protected override void ComposeBody(GuiComposer composer)
        {
            var labelFont = CairoFont.WhiteSmallText();
            var topBounds = ElementBounds.FixedSize(400, 30);
            var left = ElementBounds.FixedSize(100, 30).FixedUnder(topBounds, 10);
            var right = ElementBounds.FixedSize(270, 30).FixedUnder(topBounds, 10).FixedRightOf(left, 10);
            var controlRowBoundsLeftFixed = ElementBounds.FixedSize(100, 30).WithAlignment(EnumDialogArea.LeftFixed);
            var controlRowBoundsRightFixed = ElementBounds.FixedSize(100, 30).WithAlignment(EnumDialogArea.RightFixed);

            composer
                .AddStaticText(LangEntry("lblVolume"), labelFont, EnumTextOrientation.Right, left)
                .AddHoverText(LangEntry("lblVolume.HoverText"), labelFont, 260, left)
                .AddSlider(OnVolumeChanged, right, "sldVolume");


            left = ElementBounds.FixedSize(100, 30).FixedUnder(left, 10);
            right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

            // NOTE: Can't implement Pitch Shifting, until the LoadedSound class is de-obfuscated.
            //composer
            //    .AddStaticText(LangEntry("lblPitch"), labelFont, EnumTextOrientation.Right, left)
            //    .AddHoverText(LangEntry("lblPitch.HoverText"), labelFont, 260, left)
            //    .AddSlider(OnPitchChanged, right, "sldPitch");

            //left = ElementBounds.FixedSize(100, 30).FixedUnder(left, 10);
            //right = ElementBounds.FixedSize(270, 30).FixedUnder(right, 10).FixedRightOf(left, 10);

            composer
                .AddStaticText(LangEntry("lblMute"), labelFont, EnumTextOrientation.Right, left)
                .AddHoverText(LangEntry("lblMute.HoverText"), labelFont, 260, left)
                .AddSwitch(OnMuteToggle, right, "btnMute");

            composer.AddSmallButton(LangEntry("PlaySound"), OnPlayButtonPressed, controlRowBoundsLeftFixed.FixedUnder(right, 10), EnumButtonStyle.Normal, EnumTextOrientation.Center, "btnPlaySound")
                .AddSmallButton(LangEx.GetCore("confirmation-ok"), OnOkButtonPressed, controlRowBoundsRightFixed.FixedUnder(right, 10));
        }

        private bool OnPlayButtonPressed()
        {
            var asset = AssetLocation.Create(_cell.Path);

            if (asset.Category == AssetCategory.sounds)
            {
                ApiEx.Client.World.PlaySoundFor(asset, ApiEx.Client.World.Player, true, 0f, _cell.Muted ? 0f : _cell.VolumeMultiplier);
            }

            if (asset.Category != AssetCategory.music) return true;

            ApiEx.Client.CurrentMusicTrack?.FadeOut(0);
            ApiEx.Client.GetInternalClientSystem<SystemMusicEngine>().StartTrack(asset, 1000, EnumSoundType.Sound);
            return true;
        }

        private bool OnOkButtonPressed()
        {
            OnOkAction?.Invoke(_cell);
            return TryClose();
        }

        private bool OnVolumeChanged(int value)
        {
            _cell.VolumeMultiplier = value / 100f;
            return true;
        }

        // NOTE: Can't implement Pitch Shifting, until the LoadedSound class is de-obfuscated.
        //private bool OnPitchChanged(int value)
        //{
        //    _cell.PitchMultiplier = value / 100f;
        //    return true;
        //}

        private void OnMuteToggle(bool state)
        {
            _cell.Muted = state;
        }
    }
}
