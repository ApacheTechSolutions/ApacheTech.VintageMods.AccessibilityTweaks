using ApacheTech.VintageMods.AccessibilityTweaks.Features.SoundEffects.Dialogue;
using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.AccessibilityTweaks.Infrastructure.VolumeOverride.Dialogue
{
    /// <summary>
    ///     Defines the information stored within a single GUI Cell Element.
    ///     A list of these cells is displayed in <see cref="SoundEffectsDialogue"/>,
    ///     to allow the user to import waypoints from a JSON file.
    /// </summary>
    /// <seealso cref="SavegameCellEntry" />
    public sealed class VolumeOverrideCellEntry : SavegameCellEntry
    {
        /// <summary>
        ///     Gets the DTO model that defines the structure of the JSON import file.
        /// </summary>
        public VolumeOverride Model { get; init; }
    }
}