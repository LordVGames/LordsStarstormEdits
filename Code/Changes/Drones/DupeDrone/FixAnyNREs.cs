using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Drones.DupeDrone;


[MonoDetourTargets]
internal static class FixAnyNREs
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.CloneDrone.CloneDroneSearch.Search.ILHook(FixSearchNRE);
    }


    private static void FixSearchNRE(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        Instruction startOfSkip = null!;
        Instruction endOfSkip = null!;
        Mono.Cecil.MethodReference loadColliderInstructionReference = null!;


        w.MatchRelaxed(
            x => x.MatchCallOrCallvirt(out loadColliderInstructionReference),
            x => x.MatchCallOrCallvirt<Component>("get_transform") && w.SetCurrentTo(x) && w.SetInstructionTo(ref startOfSkip, x),
            x => x.MatchCallOrCallvirt<Transform>("get_parent"),
            x => x.MatchCallOrCallvirt<Component>("get_gameObject"),
            x => x.MatchStloc(out _),
            x => x.MatchLdloc(out _),
            x => x.MatchLdloc(out _),
            x => x.MatchCallOrCallvirt(out _) && w.SetInstructionTo(ref endOfSkip, x)
        ).ThrowIfFailure()
        .InsertBranchOverIfTrue(startOfSkip, endOfSkip,
            w.CreateDelegateCall((Collider collider) =>
            {
                // SS2 NREs here because some item transforms apparently have (to quote wheatley) "well um...lack of parent(s)"
                return collider == null
                    || collider.transform == null
                    || collider.transform.parent == null;
            })
        )
        // weaver current is on the get_transform line still
        .InsertBeforeCurrent(
            w.Create(OpCodes.Ldloca, 3),
            w.Create(OpCodes.Call, loadColliderInstructionReference)
        );
    }
}