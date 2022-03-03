using ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks.Dialogue;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.VisualTweaks
{
    /// <summary>
    ///     - Feature: Visual Tweaks<br/><br/>
    ///
    ///     Various effects within the game can be harmful, and even potentially fatal to some players.<br/>
    ///     This mod helps to alleviate some of these issues by allowing the player to disable various rendered effects and sound effects.<br/><br/>
    ///
    ///         - Disable Raindrop Particles<br/>
    ///         - Disable Hailstone Particles<br/>
    ///         - Disable Snowflake Particles<br/>
    ///         - Disable Dust Particles<br/>
    ///         - Disable Lightning Flashes<br/>
    ///         - Disable Cloud Rendering<br/>
    ///         - Disable Fog Rendering<br/>
    ///         - Disable Camera Shaking (Shivering, etc.)<br/>
    ///         - Disable Temporal Storm Warp Effect
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class VisualTweaks : ClientModSystem
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client-side API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            ClientSettings.RenderClouds = ModServices.IOC.Resolve<VisualTweaksSettings>().CloudsEnabled;

            FluentChat.ClientCommand("actweaks")
                .RegisterWith(api)
                .HasDescription(LangEx.FeatureString(nameof(VisualTweaks), "Description"))
                .HasDefaultHandler((_, _) => ModServices.IOC.Resolve<VisualTweaksDialogue>().Toggle());
        }
    }


}