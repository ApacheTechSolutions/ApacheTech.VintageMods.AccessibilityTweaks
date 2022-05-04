using System;
using System.Collections.Generic;
using System.Linq;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride.Dialogue;
using ApacheTech.VintageMods.Core.Abstractions.GUI;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.Game;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Dialogue
{
    /// <summary>
    ///     Dialogue Window: Allows the user to import waypoints from JSON files.
    /// </summary>
    /// <seealso cref="GenericDialogue" />
    public sealed class SoundEffectsDialogue : FeatureSettingsDialogue<SoundEffectsSettings>
    {
        private ElementBounds _clippedBounds;
        private ElementBounds _cellListBounds;
        private readonly Queue<ILoadedSound> _activeSounds;

        private List<VolumeOverrideCellEntry> _cells = new();

        private GuiElementCellList<VolumeOverrideCellEntry> _cellList;
        private bool _activeSoundsOnly;
        private string _filterString;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SoundEffectsDialogue" /> class.
        /// </summary>
        /// <param name="capi">Client API pass-through</param>
        /// <param name="settings"></param>
        public SoundEffectsDialogue(ICoreClientAPI capi, SoundEffectsSettings settings) : base(capi, settings)
        {
            Alignment = EnumDialogArea.CenterMiddle;
            _activeSounds = ApiEx.ClientMain.GetField<Queue<ILoadedSound>>("ActiveSounds");
            
            ClientSettings.Inst.AddWatcher<float>("guiScale", _ =>
            {
                Compose();
                RefreshValues();
            });
        }

        #region Form Composition

        /// <summary>
        ///     Composes the GUI components for this instance.
        /// </summary>
        protected override void Compose()
        {
            base.Compose();
            RefreshCells();
        }

        private void RefreshCells()
        {
            _cells = GetCellEntries();
            _cellList.ReloadCells(_cells);
            UpdateActiveSounds();
            FilterCells();
            RefreshValues();
        }

        private void UpdateActiveSounds()
        {
            foreach (var sound in _activeSounds)
            {
                sound.SetVolume();
            }
        }

        /// <summary>
        ///     Refreshes the displayed values on the form.
        /// </summary>
        protected override void RefreshValues()
        {
            if (SingleComposer is null) return;
            _cellListBounds.CalcWorldBounds();
            _clippedBounds.CalcWorldBounds();
            SingleComposer.GetScrollbar("scrollbar").SetHeights((float)_clippedBounds.fixedHeight, (float)_cellListBounds.fixedHeight);
        }

        private List<VolumeOverrideCellEntry> GetCellEntries()
        {
            var list = new List<VolumeOverrideCellEntry>();
            foreach (var dto in Settings.SoundAssets.Values)
            {
                try
                {
                    list.Add(new VolumeOverrideCellEntry
                    {
                        Title = dto.Path,
                        DetailText = "",
                        Enabled = !dto.Muted,
                        RightTopText = "",
                        RightTopOffY = 3f,
                        DetailTextFont = CairoFont.WhiteDetailText().WithFontSize((float)GuiStyle.SmallFontSize),
                        Model = dto
                    });
                }
                catch (Exception exception)
                {
                    ApiEx.Client.Logger.Error("[VintageMods] Error caught while loading sound effects from settings file.");
                    ApiEx.Client.Logger.Error(exception.Message);
                    ApiEx.Client.Logger.Error(exception.StackTrace);
                    return new List<VolumeOverrideCellEntry>();
                }
            }
            return list;
        }

        /// <summary>
        ///     Composes the main body of the dialogue window.
        /// </summary>
        /// <param name="composer">The GUI composer.</param>
        protected override void ComposeBody(GuiComposer composer)
        {
            var platform = capi.World.GetField<ClientPlatformAbstract>("Platform");
            var scaledWidth = Math.Min(800, platform.WindowSize.Width * 0.5) / ClientSettings.GUIScale;
            var scaledHeight = Math.Min(600, (platform.WindowSize.Height - 65) * 0.85) / ClientSettings.GUIScale;

            var controlRowBoundsCentreFixed = ElementBounds
                .FixedSize(100, 30)
                .WithFixedPadding(10, 2)
                .WithAlignment(EnumDialogArea.CenterFixed);

            var outerBounds = ElementBounds
                .Fixed(EnumDialogArea.LeftTop, 0, 0, scaledWidth, 35);

            AddSearchBox(composer, ref outerBounds);

            var insetBounds = outerBounds
                .BelowCopy(0, 3)
                .WithFixedSize(scaledWidth, scaledHeight);

            _clippedBounds = insetBounds
                .ForkContainingChild(3, 3, 3, 3);

            _cellListBounds = _clippedBounds
                .ForkContainingChild(0.0, 0.0, 0.0, -3.0)
                .WithFixedPadding(10.0);

            _cellList = new GuiElementCellList<VolumeOverrideCellEntry>(capi, _cellListBounds, OnRequireCell, _cells);
            
            composer
                .AddInset(insetBounds)
                .AddVerticalScrollbar(OnScroll, ElementStdBounds.VerticalScrollbar(insetBounds), "scrollbar")
                .BeginClip(_clippedBounds)
                .AddInteractiveElement(_cellList)
                .EndClip()
                
                .AddSmallButton(LangEntry("RightButtonText"), OnRightButtonPressed,
                    controlRowBoundsCentreFixed.FixedUnder(insetBounds, 10.0));
        }
        
        private void AddSearchBox(GuiComposer composer, ref ElementBounds bounds)
        {
            const int switchSize = 30;
            const int gapBetweenRows = 20;
            var font = CairoFont.WhiteSmallText();
            var lblSearchText = LangEntry("lblSearch");
            var lblCurrentlyPlayingText = LangEntry("lblCurrentlyPlaying");

            var lblSearchTextLength = font.GetTextExtents(lblSearchText).Width + 10;
            var lblCurrentlyPlayingTextLength = font.GetTextExtents(lblCurrentlyPlayingText).Width + 10;

            var left = ElementBounds.Fixed(0, 5, lblSearchTextLength, switchSize).FixedUnder(bounds, 3);
            var right = ElementBounds.Fixed(lblSearchTextLength + 10, 0, 200, switchSize).FixedUnder(bounds, 3);

            composer.AddStaticText(lblSearchText, font, EnumTextOrientation.Left, left);
            composer.AddAutoSizeHoverText(LangEntry("lblSearch.HoverText"), font, 160, left);
            composer.AddTextInput(right, OnFilterTextChanged);

            right = ElementBounds.FixedSize(EnumDialogArea.RightFixed, switchSize, switchSize).FixedUnder(bounds, 3);
            left = ElementBounds.FixedSize(EnumDialogArea.RightFixed, lblCurrentlyPlayingTextLength, switchSize).FixedUnder(bounds, 8).WithFixedOffset(-40, 0);

            composer.AddStaticText(lblCurrentlyPlayingText, font, EnumTextOrientation.Left, left);
            composer.AddSwitch(OnCurrentlyPlayingToggle, right);

            bounds = bounds.BelowCopy(fixedDeltaY: gapBetweenRows);
        }

        #endregion

        #region Control Event Handlers

        private void OnCurrentlyPlayingToggle(bool state)
        {
            _activeSoundsOnly = state;
            FilterCells();
            RefreshValues();
        }

        private void OnFilterTextChanged(string filterString)
        {
            _filterString = filterString;
            FilterCells();
            RefreshValues();
        }

        private void FilterCells()
        {
            bool Filter(IGuiElementCell cell)
            {
                return (string.IsNullOrWhiteSpace(_filterString) || ((SoundEffectsGuiCell)cell).Cell.Title.Contains(_filterString)) &&
                       (!_activeSoundsOnly || _activeSounds.Any(p => p.Params.Location.ToString() == ((SoundEffectsGuiCell)cell).Cell.Title));
            }

            _cellList.CallMethod("FilterCells", (Func<IGuiElementCell, bool>)Filter);
        }

        /// <summary>
        ///     Called when the GUI needs to refresh or create a cell to display to the user. 
        /// </summary>
        private IGuiElementCell OnRequireCell(VolumeOverrideCellEntry cell, ElementBounds bounds)
        {
            return new SoundEffectsGuiCell(ApiEx.Client, cell, bounds)
            {
                On = !cell.Model.Muted,
                OnMouseDownOnCellLeft = OnCellClickLeft,
                OnMouseDownOnCellRight = OnCellClickRight
            };
        }

        private void OnCellClickLeft(int val)
        {
            var cell = _cellList.elementCells.Cast<SoundEffectsGuiCell>().ToList()[val];
            var dialogue = ModServices.IOC.CreateInstance<EditVolumeDialogue>(cell.Cell.Model).With(p =>
            {
                p.OnOkAction = result =>
                {
                    Settings.SoundAssets[result.Path] = result;
                    SaveFeatureChanges();
                    RefreshCells();
                };
            });
            dialogue.TryOpen();
        }

        /// <summary>
        ///     Called when the user clicks on one of the cells in the grid.
        /// </summary>
        private void OnCellClickRight(int val)
        {
            var cell = _cellList.elementCells.Cast<SoundEffectsGuiCell>().ToList()[val];
            cell.On = !cell.On;
            cell.Enabled = cell.On;
            cell.Cell.Model.Muted = !cell.On;
            Settings.SoundAssets[cell.Cell.Model.Path].Muted = !cell.On;
            SaveFeatureChanges();
            RefreshValues();
            UpdateActiveSounds();
        }

        private void OnScroll(float dy)
        {
            var bounds = _cellList.Bounds;
            bounds.fixedY = 0f - dy;
            bounds.CalcWorldBounds();
        }
        
        private static bool OnRightButtonPressed()
        {
            ApiEx.ClientMain.StopAllSounds();
            return true;
        }

        #endregion
    }
}