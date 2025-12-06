using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace LordsStarstormEdits.SS2Edits.EtherealStuff;


[MonoDetourTargets(typeof(TradeController), GenerateControlFlowVariants = true)]
internal static class ShardTradeDropFix
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }

        Mdh.SS2.TradeController.InitPrefabTEMP.ControlFlowPrefix(JustUseTheNormalPotential);
    }

    private static ReturnFlow JustUseTheNormalPotential()
    {
        TradeController.pickupPrefablol = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion();
        return ReturnFlow.SkipOriginal;
    }
}