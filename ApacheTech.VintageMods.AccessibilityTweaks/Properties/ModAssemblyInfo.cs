using ApacheTech.VintageMods.Core.Annotation.Attributes;
using Vintagestory.API.Common;

// ReSharper disable StringLiteralTypo

[assembly:ModDependency("game", "1.16.0-rc.4")]

[assembly:ModInfo(
    "Accessibility Tweaks",
    "accessibilitytweaks",
    Description = "Quality of Life changes to aid content creators, and those with motion/light/noise affected epilepsy, or light sensitivity.",
    Side = "Client",
    Version = "2.0.0-rc.4",
    NetworkVersion = "1.0.0",
    IconPath = "modicon.png",
    Website = "https://apachetech.co.uk",
    Contributors = new[] { "ApacheTech Solutions" },
    Authors = new []{ "ApacheTech Solutions" })]

[assembly: VintageModInfo(
    ModId = "accessibilitytweaks",
    ModName = "Accessibility Tweaks",
    RootDirectoryName = "AccessibilityTweaks",
    NetworkVersion = "1.0.0",
    Version = "2.0.0", Side = EnumAppSide.Client)]