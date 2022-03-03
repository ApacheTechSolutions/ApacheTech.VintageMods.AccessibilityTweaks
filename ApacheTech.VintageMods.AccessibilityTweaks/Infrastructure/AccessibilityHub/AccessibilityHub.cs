using ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.AccessibilityHub.Dialogue;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.Core.Extensions.Game;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.AccessibilityHub
{
    /// <summary>
    ///     Provides a main GUI for the mod, as a central location to access each feature; rather than through commands.
    /// </summary>
    /// <seealso cref="ClientModSystem" />
    public sealed class AccessibilityHub :  ClientModSystem
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The game's core client API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Input.RegisterTransientGuiDialogueHotKey<AccessibilityHubDialogue>(LangEx.ModTitle(), GlKeys.F8);
        }
    }
}
