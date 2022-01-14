using ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects.Dialogue;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.FluentChatCommands;
using Vintagestory.API.Client;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.RenderedEffects
{
    /// <summary>
    ///     Feature: Rendered Effects.
    /// 
    ///      • Toggle Rain Particle Effects
    ///      • Toggle Hail Particle Effects
    ///      • Toggle Snow Particle Effects
    ///      • Toggle Dust Particle Effects 
    ///      • Toggle Cloud Render Effects
    ///      • Toggle Weather Sound Effects 
    ///      • Toggle Fog Effects
    ///      • Toggle Glitch Render Effects
    ///      • Toggle Lightning Lighting Effects
    ///      • Change Volume of Rifts
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class RenderedEffects : ClientModSystem
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client-side API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            FluentChat.ClientCommand("actweaks")
                .RegisterWith(api)
                .HasDescription(LangEx.FeatureString("RenderedEffects", "Description"))
                .HasDefaultHandler((_, _) => ModServices.IOC.Resolve<RenderedEffectsDialogue>().TryOpen());
        }
    }
}
