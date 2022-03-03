using ApacheTech.VintageMods.Core.Abstractions.Features;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Patches
{
    /// <summary>
    ///     Harmony Patches for the <see cref="ShaderRegistry"/> class. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="VisualTweaksSettings" />
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class ShaderRegistryPatches : WorldSettingsConsumer<VisualTweaksSettings>
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPostfix"/> patch to the "onServerData" method of the <see cref="SystemTemporalStability"/> class.
        /// </summary>
        /// <param name="program">The instance of <see cref="ShaderProgramBase"/> passed into the original method.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShaderRegistry), "registerDefaultShaderCodePrefixes")]
        public static void Patch_ShaderRegistry_registerDefaultShaderCodePrefixes_Prefix(ShaderProgramBase program)
        {
            var stabilitySystem = ApiEx.Client.ModLoader.GetModSystem<SystemTemporalStability>();
            if (!stabilitySystem.StormData.nowStormActive) return;
            var value = Settings.GlitchEnabled ? 1 : 0;
            program.VertexShader.PrefixCode = program.VertexShader.PrefixCode.Replace("#define WAVINGSTUFF 1", $"#define WAVINGSTUFF {value}");
        }
    }
}