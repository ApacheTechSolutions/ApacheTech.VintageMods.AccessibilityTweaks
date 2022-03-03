using ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection.Shaders;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Extensions;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.ColourCorrection
{
    /// <summary>
    ///  - Feature: Colour Correction
    ///  
    ///     Helps people with poor vision, light sensitivity, or colour-blindness.Playing around with the colour settings can give everything a warm, or cool hue.
    ///  
    ///     - Choose from different presets to simulate different colour vision deficiencies.
    ///     - Adjust the colour balance, and saturation of the game scene.
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class ColourCorrection : ClientModSystem
    {
        private ICoreClientAPI _capi;
        private ColourCorrectionRenderer _renderer;

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="capi">The core client-side API.</param>
        public override void StartClientSide(ICoreClientAPI capi)
        {
            _capi = capi;
            _renderer = ModServices.IOC.Resolve<ColourCorrectionRenderer>();
            _capi.Event.RegisterRenderer(_renderer, EnumRenderStage.AfterFinalComposition);
            _capi.Event.ReloadShader += LoadShader;
            LoadShader();
        }

        private bool LoadShader()
        {
            var shader = ModServices.IOC.Resolve<ColourCorrectionShaderProgram>();
            _capi.Shader.RegisterFileShaderProgram(shader.PassName, shader);
            shader.Compile();
            _renderer.Shader = shader;
            return true;
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers
        /// </summary>
        public override void Dispose()
        {
            _capi.Event.ReloadShader -= LoadShader;
            _capi.Event.UnregisterRenderer(_renderer, EnumRenderStage.AfterFinalComposition);
            _capi.Shader.ReloadShadersThreadSafe();
            _renderer?.Dispose();
            base.Dispose();
        }
    }
}