using System;
using System.Runtime.CompilerServices;
using RoR2;

namespace LordsStarstormEdits;

internal static class ModSoftDependencies
{
    internal static class LordsItemEditsMod
    {
        private static bool? _enabled;
        internal static bool ModIsRunning
        {
            get
            {
                _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LordsItemEdits.Plugin.Id);
                return (bool)_enabled;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static float GetEditedICBMDamageMult(CharacterBody victimBody)
        {
            return LordsItemEdits.ItemEdits.PocketICBM.GetICBMDamageMult(victimBody);
        }
    }
}