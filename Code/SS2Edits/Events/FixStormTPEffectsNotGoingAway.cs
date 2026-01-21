using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using HarmonyLib;
namespace StarstormSquared.SS2Edits.Events;

[MonoDetourTargets(typeof(EntityStates.Events.Storm))]
internal static class FixStormTPEffectsNotGoingAway
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        Mdh.EntityStates.Events.Storm.FixedUpdate.ILHook(RemoveStormEffects);
    }

    private static void RemoveStormEffects(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel returnLabel = w.DefineLabel();

        w.MatchRelaxed(
            x => x.MatchLdarg(0),
            x => x.MatchLdfld<EntityStates.EntityState>("outer"),
            x => x.MatchNewobj(out _),
            x => x.MatchCallvirt<EntityStateMachine>("SetNextState") && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldsfld, AccessTools.DeclaredField(typeof(TeleporterUpgradeController), nameof(TeleporterUpgradeController.instance)))
        )
        .InsertAfterCurrent(
            w.CreateDelegateCall(
                (TeleporterUpgradeController teleporterUpgradeController) =>
                {
                    // why was this line removed? im gonna guess it's by accident ngl
                    if (teleporterUpgradeController) teleporterUpgradeController.UpgradeStorm(false);
                }
            )
        );
    }
}