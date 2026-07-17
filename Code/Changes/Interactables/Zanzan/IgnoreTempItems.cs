using HG;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using SS2.Components;
using SS2.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
namespace StarstormSquared.Changes.Interactables.Zanzan;


[MonoDetourTargets(typeof(TraderController))]
internal static class IgnoreTempItems
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.TraderController.AssignPotentialInteractor.ILHook(CheckForTemp);
    }


    private static void CheckForTemp(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipInForLoopLabel = w.DefineLabel();


        w.MatchRelaxed(
            x => x.MatchLdloc(4),
            x => x.MatchLdsfld<PickupIndex>("none"),
            x => x.MatchCallOrCallvirt(out _),
            x => x.MatchBrfalse(out skipInForLoopLabel) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc_1),
            w.Create(OpCodes.Ldloc_3),
            w.CreateDelegateCall((Inventory inventory, int itemAcquisitionNumber) =>
            {
                // zanzan can still do proper trades if you have any of the real version of an item alongside temp ones
                ItemIndex currentItemIndex = inventory.itemAcquisitionOrder[itemAcquisitionNumber];
                return inventory.GetItemCountTemp(currentItemIndex) > 0 && inventory.GetItemCountPermanent(currentItemIndex) < 1;
            }),
            w.Create(OpCodes.Brtrue, skipInForLoopLabel)
        );
    }
}