using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events.EliteEvents;


[MonoDetourTargets]
internal static class FixMultiplayerShardDrops
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EliteEventMissionController.OnBossKilledServer.OnKilledServer.ILHook(PreventDropChange);
    }


    private static void PreventDropChange(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        w.MatchRelaxed(
            x => x.MatchLdloc(3) && w.SetCurrentTo(x),
            x => x.MatchLdarg(1),
            x => x.MatchLdfld<DamageReport>("victimBody"),
            x => x.MatchCallOrCallvirt(out _)
        ).ThrowIfFailure()
        .ReplaceCurrent(w.Create(OpCodes.Ldarg_0))
        .InsertAfterCurrent(
            w.Create<EliteEventMissionController.OnBossKilledServer>(OpCodes.Ldfld, "drop")
        );
    }
}