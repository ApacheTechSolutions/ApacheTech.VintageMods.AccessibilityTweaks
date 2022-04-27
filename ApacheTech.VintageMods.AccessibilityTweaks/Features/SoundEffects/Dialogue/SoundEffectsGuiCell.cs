using System;
using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride.Dialogue;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Cairo;
using Vintagestory.API.Client;

// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Dialogue
{
    /// <summary>
    ///     A cell displayed within the cell list on the <see cref="SoundEffectsDialogue"/> screen.
    /// </summary>
    /// <seealso cref="GuiElementTextBase" />
    /// <seealso cref="IGuiElementCell" />
    public sealed class SoundEffectsGuiCell : GuiElementTextBase, IGuiElementCell
    {
        private LoadedTexture _cellTexture;
        private int _switchOnTextureId;
        private int _leftHighlightTextureId;
        private int _rightHighlightTextureId;
        
        private const double UnscaledSwitchSize = 25.0;
        private const double UnscaledSwitchPadding = 4.0;
        private const double UnscaledRightBoxWidth = 40.0;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="SoundEffectsGuiCell" /> class.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="bounds">The bounds.</param>
        public SoundEffectsGuiCell(ICoreClientAPI capi, VolumeOverrideCellEntry cell, ElementBounds bounds) : base(capi, "", null, bounds)
        {
            Cell = cell;
            Bounds = bounds.WithFixedHeight(30);
            _cellTexture = new LoadedTexture(capi);
            Cell.TitleFont ??= CairoFont.WhiteSmallishText();
            Cell.RightTopOffY = 3f;

            if (Cell.DetailTextFont != null) return;
            Cell.DetailTextFont = CairoFont.WhiteDetailText();
            Cell.DetailTextFont.Color[3] *= 0.6;
        }

        /// <summary>
        /// 	The cell entry that contains the data to display within this <see cref="IGuiElementCell"/>.
        /// </summary>
        public VolumeOverrideCellEntry Cell { get; }

        private void GenerateEnabledTexture()
        {
            var size = scaled(UnscaledSwitchSize - 2.0 * UnscaledSwitchPadding);
            using var imageSurface = new ImageSurface(0, (int)size, (int)size);
            using var context = genContext(imageSurface);
            RoundRectangle(context, 0.0, 0.0, size, size, 2.0);
            fillWithPattern(api, context, waterTextureName);
            generateTexture(imageSurface, ref _switchOnTextureId);
        }

        private void Compose()
        {
            ComposeHover(true, ref _leftHighlightTextureId);
            ComposeHover(false, ref _rightHighlightTextureId);
            GenerateEnabledTexture();
            using var imageSurface = new ImageSurface(0, Bounds.OuterWidthInt, Bounds.OuterHeightInt);
            using var context = new Context(imageSurface);
            var num = scaled(UnscaledRightBoxWidth);
            Bounds.CalcWorldBounds();

            // Form
            const double brightness = 1.2;
            RoundRectangle(context, 0.0, 0.0, Bounds.OuterWidth, Bounds.OuterHeight, 0.0);
            context.SetSourceRGBA(GuiStyle.DialogDefaultBgColor[0] * brightness, GuiStyle.DialogDefaultBgColor[1] * brightness, GuiStyle.DialogDefaultBgColor[2] * brightness, 1);
            context.Paint();

            // Main Title.
            Font = Cell.TitleFont;
            textUtil.AutobreakAndDrawMultilineTextAt(context, Font, Cell.Title, Bounds.absPaddingX, Bounds.absPaddingY + scaled(Cell.RightTopOffY), Bounds.InnerWidth);

            // Top Right Text: Location of Waypoint, relative to spawn.
            Font = Cell.DetailTextFont.WithLineHeightMultiplier(1.5);
            var textExtents = Font.GetTextExtents(Cell.RightTopText ?? "");
            textUtil.AutobreakAndDrawMultilineTextAt(context, Font, Cell.RightTopText,
                Bounds.absPaddingX + Bounds.InnerWidth - textExtents.Width - num - scaled(10.0), Bounds.absPaddingY + scaled(Cell.RightTopOffY), textExtents.Width + 1.0, EnumTextOrientation.Right);

            context.Operator = Operator.Add;
            EmbossRoundRectangleElement(context, 0.0, 0.0, Bounds.OuterWidth, Bounds.OuterHeight, false, 4, 0);

            var scaledSwitchSize = scaled(UnscaledSwitchSize);
            var scaledSwitchPadding = scaled(UnscaledSwitchPadding);
            var x = Bounds.absPaddingX + Bounds.InnerWidth - scaled(0.0) - scaledSwitchSize - scaledSwitchPadding;
            var y = Bounds.absPaddingY + Bounds.absPaddingY;
            context.SetSourceRGBA(0.0, 0.0, 0.0, 0.2);
            RoundRectangle(context, x, y, scaledSwitchSize, scaledSwitchSize, 3.0);
            context.Fill();
            EmbossRoundRectangleElement(context, x, y, scaledSwitchSize, scaledSwitchSize, true, 1, 2);

            generateTexture(imageSurface, ref _cellTexture);
        }

        private void ComposeHover(bool left, ref int textureId)
        {
            var imageSurface = new ImageSurface(0, (int)Bounds.OuterWidth, (int)Bounds.OuterHeight);
            var context = genContext(imageSurface);
            var num = scaled(UnscaledRightBoxWidth);
            if (left)
            {
                context.NewPath();
                context.LineTo(0.0, 0.0);
                context.LineTo(Bounds.InnerWidth - num, 0.0);
                context.LineTo(Bounds.InnerWidth - num, Bounds.OuterHeight);
                context.LineTo(0.0, Bounds.OuterHeight);
                context.ClosePath();
            }
            else
            {
                context.NewPath();
                context.LineTo(Bounds.InnerWidth - num, 0.0);
                context.LineTo(Bounds.OuterWidth, 0.0);
                context.LineTo(Bounds.OuterWidth, Bounds.OuterHeight);
                context.LineTo(Bounds.InnerWidth - num, Bounds.OuterHeight);
                context.ClosePath();
            }
            context.SetSourceRGBA(0.0, 0.0, 0.0, 0.15);
            context.Fill();
            generateTexture(imageSurface, ref textureId);
            context.Dispose();
            imageSurface.Dispose();
        }

        /// <summary>
        ///     Called when the GUI Composer is ready to render human interactive elements.
        /// </summary>
        /// <param name="capi">The capi.</param>
        /// <param name="deltaTime">The delta time.</param>
        public void OnRenderInteractiveElements(ICoreClientAPI capi, float deltaTime)
        {
            if (_cellTexture.TextureId == 0)
            {
                Compose();
            }
            api.Render.Render2DTexturePremultipliedAlpha(_cellTexture.TextureId, (int)Bounds.absX, (int)Bounds.absY, Bounds.OuterWidthInt, Bounds.OuterHeightInt);
            var mouseX = api.Input.MouseX;
            var mouseY = api.Input.MouseY;
            var vec2d = Bounds.PositionInside(mouseX, mouseY);
            if (vec2d != null)
            {
                api.Render.Render2DTexturePremultipliedAlpha(
                    vec2d.X > Bounds.InnerWidth - scaled(GuiElementMainMenuCell.unscaledRightBoxWidth)
                        ? _rightHighlightTextureId
                        : _leftHighlightTextureId, (int)Bounds.absX, (int)Bounds.absY, Bounds.OuterWidth,
                    Bounds.OuterHeight);
            }
            if (On)
            {
                var num = scaled(UnscaledSwitchSize - 2.0 * UnscaledSwitchPadding);
                var num2 = scaled(UnscaledSwitchPadding);
                var posX = Bounds.renderX + Bounds.InnerWidth - num + num2 - scaled(5.0);
                var posY = Bounds.renderY + scaled(8.0) + num2;
                api.Render.Render2DTexturePremultipliedAlpha(_switchOnTextureId, posX, posY, (int)num, (int)num);
                return;
            }
            api.Render.Render2DTexturePremultipliedAlpha(_rightHighlightTextureId, (int)Bounds.renderX, (int)Bounds.renderY, Bounds.OuterWidth, Bounds.OuterHeight);
            api.Render.Render2DTexturePremultipliedAlpha(_leftHighlightTextureId, (int)Bounds.renderX, (int)Bounds.renderY, Bounds.OuterWidth, Bounds.OuterHeight);
        }

        /// <summary>
        ///     Called when the cell is modified and needs to be updated.
        /// </summary>
        public void UpdateCellHeight()
        {
            /* VANILLA API */
        }

        /// <summary>
        ///     Called when a MouseDown event is invoked on the element.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="elementIndex">Index of the element.</param>
        public void OnMouseDownOnElement(MouseEvent args, int elementIndex)
        {
            /* VANILLA API */
        }

        /// <summary>
        ///     Called when a MouseMove event is invoked on the element.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="elementIndex">Index of the element.</param>
        public void OnMouseMoveOnElement(MouseEvent args, int elementIndex)
        {
            /* VANILLA API */
        }

        /// <summary>
        ///     Called when a MouseUp event is invoked on the element.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="elementIndex">Index of the element.</param>
        public void OnMouseUpOnElement(MouseEvent args, int elementIndex)
        {
            var mouseX = api.Input.MouseX;
            var mouseY = api.Input.MouseY;
            var vec2d = Bounds.PositionInside(mouseX, mouseY);
            api.Gui.PlaySound("menubutton_press");
            if (vec2d.X > Bounds.InnerWidth - scaled(GuiElementMainMenuCell.unscaledRightBoxWidth))
            {
                OnMouseDownOnCellRight?.Invoke(elementIndex);
                args.Handled = true;
                return;
            }
            OnMouseDownOnCellLeft?.Invoke(elementIndex);
            args.Handled = true;
        }

        /// <inheritdoc />
        public new ElementBounds Bounds { get; }

        /// <summary>
        ///     Called when the user clicks on the left side of the cell.
        /// </summary>
        public Action<int> OnMouseDownOnCellLeft { private get; init; }

        /// <summary>
        ///     Called when the user clicks on the right side of the cell.
        /// </summary>
        public Action<int> OnMouseDownOnCellRight { private get; init; }

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public bool On { get; set; } = true;

        /// <summary>
        ///     Disposes this instance.
        /// </summary>
        public override void Dispose()
        {
            ApiEx.ClientMain.EnqueueMainThreadTask(() =>
            {
                _cellTexture?.Dispose();
                api.Render.GLDeleteTexture(_leftHighlightTextureId);
                api.Render.GLDeleteTexture(_rightHighlightTextureId);
                api.Render.GLDeleteTexture(_switchOnTextureId);
                base.Dispose();
            }, "");
        }
    }
}