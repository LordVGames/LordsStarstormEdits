using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Elites.SuperElites;


[MonoDetourTargets]
internal static class DropShardBasedOnEliteType
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EliteEventMissionController.OnBossKilledServer.OnKilledServer.ILHook(SkipOverShardDrop);
        //Mdh.SS2.EliteEventMissionController.Awake.Postfix(ShowBossDrop);
    }

    /*private static void ShowBossDrop(EliteEventMissionController self)
    {
        Log.Warning($"self.bossDrop is {self.bossDrop == null}");
        Log.Warning($"self.bossEliteEquipmentis {self.bossEliteEquipment == null}");
        Log.Warning($"self.bossDrop is {self.bossDrop.name}");
        Log.Warning($"self.bossEliteEquipmentis {self.bossEliteEquipment.name}");
    }*/

    private static void SkipOverShardDrop(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        Instruction startOfSkip = null!;
        Instruction endOfSkip = null!;


        w.MatchRelaxed(
            x => x.MatchLdsfld("SS2.SS2Content/Items", "ShardStorm") && w.SetInstructionTo(ref startOfSkip, x) && w.SetCurrentTo(x),
            x => x.MatchCallvirt(out _),
            x => x.MatchCall(out _),
            x => x.MatchStloc(3) && w.SetInstructionTo(ref endOfSkip, x)
        ).ThrowIfFailure()
        .InsertBranchOver(startOfSkip, endOfSkip);
    }
}