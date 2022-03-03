using ApacheTech.VintageMods.Core.Abstractions.Shaders;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using Vintagestory.Client.NoObf;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection.Shaders
{
    /// <summary>
    ///     Changes the hue, and saturation of the scene colours, based on user-defined settings.
    /// </summary>
    /// <seealso cref="ShaderProgram" />
    /// <seealso cref="IGenericShaderProgram" />
    public sealed class ColourCorrectionShaderProgram : ShaderProgram, IGenericShaderProgram
    {
        private readonly ColourCorrectionSettings _settings;

        /// <summary>
        /// 	Initialises a new instance of the <see cref="ColourCorrectionShaderProgram"/> class.
        /// </summary>
        public ColourCorrectionShaderProgram(ColourCorrectionSettings settings)
        {
            AssetDomain = ApiEx.ModInfo.ModId;
            PassName = "colour-correction";
            _settings = settings;
        }

        /// <summary>
        ///     Runs before every frame, to set the values for the shader pass.
        /// </summary>
        public void UpdateUniforms()
        {
            BindTexture2D("iChannel0", ApiEx.Client.Render.FrameBuffers[0].ColorTextureIds[0], 0);
            Uniform("iColourVisionType", (int)_settings.Preset);

            Uniform("iRedBalance", _settings.Red);
            Uniform("iGreenBalance", _settings.Green);
            Uniform("iBlueBalance", _settings.Blue);
            Uniform("iSaturation", _settings.Saturation);
        }
    }
}