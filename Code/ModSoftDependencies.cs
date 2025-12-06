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
                _enabled ??= BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LordsItemEdits.Plugin.PluginGUID);
                return (bool)_enabled;
            }
        }


        internal static bool PocketICBMEditEnabled
        {
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            get
            {
                if (!ModIsRunning)
                {
                    return false;
                }
                return LordsItemEdits.ConfigOptions.PocketICBM.EnableEdit.Value;
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static float GetEditedICBMDamageMult(CharacterBody attackerBody)
        {
            if (!ModIsRunning || !PocketICBMEditEnabled)
            {
                return 1;
            }
            return LordsItemEdits.ItemEdits.PocketICBM.GetICBMDamageMult(attackerBody);
        }
    }
}