using System;
using ApacheTech.Common.Extensions.Harmony;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Abstractions;
using ApacheTech.VintageMods.Core.Services.HarmonyPatching.Annotations;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Common;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable InconsistentNaming

#pragma warning disable IDE0051

namespace ApacheTech.VintageMods.AccessibilityTweaks.Features.SuperBright.Patches
{
    [HarmonySidedPatch(EnumAppSide.Client)]
    public sealed class ClientGameCalendarPatches : SettingsConsumer<SuperBrightSettings>
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientGameCalendar), "DayLightStrength", MethodType.Getter)]
        private static void Patch_ClientGameCalendar_DayLightStrength_Postfix(IGameCalendar __instance, ref float __result)
        {
            if (!Settings.Enabled) return;
            __result = Math.Max(__result, Settings.Brightness);
            __instance.SetField("dayLight", __result);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientGameCalendar), "MoonLightStrength", MethodType.Getter)]
        private static void Patch_AmbientManager_MoonLightStrength_Postfix(ref float __result)
        {
            if (!Settings.Enabled) return;
            __result = Math.Max(__result, Settings.Brightness);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ClientGameCalendar), "SunLightStrength", MethodType.Getter)]
        private static void Patch_AmbientManager_SunLightStrength_Postfix(ref float __result)
        {
            if (!Settings.Enabled) return;
            __result = Math.Max(__result, Settings.Brightness);
        }
    }
}