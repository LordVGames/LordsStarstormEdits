using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace LordsStarstormEdits.SS2Edits.EtherealStuff
{
    [MonoDetourTargets(typeof(SS2.Items.FieldAccelerator))]
    internal static class FixFieldAcceleratorError
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            // why would you wanna config off an NRE fix

            MonoDetourHooks.SS2.Items.FieldAccelerator.FieldAcceleratorTeleporterBehavior.TeleporterInteraction_onTeleporterChargedGlobal.ILHook(NullCheckThatShitDude);
        }


        private static void NullCheckThatShitDude(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            ILLabel skipBadChecks = w.DefineLabel();

            // going in front of line:
            // displayChildLocator.FindChild("Passive").gameObject.SetActive(value: false);
            w.MatchRelaxed(
                x => x.MatchLdnull(),
                x => x.MatchCall(out _),
                x => x.MatchBrfalse(out skipBadChecks) && w.SetCurrentTo(x),
                x => x.MatchLdarg(0),
                x => x.MatchLdfld<SS2.Items.FieldAccelerator.FieldAcceleratorTeleporterBehavior>("displayChildLocator"),
                x => x.MatchLdstr("Passive")
            ).ThrowIfFailure();
            w.InsertAfterCurrent(
                w.Create(OpCodes.Ldarg_0),
                w.Create(OpCodes.Br, skipBadChecks)
            );
            w.InsertBeforeCurrent(
                w.CreateDelegateCall((ChildLocator childLocator) =>
                {
                    childLocator.FindChild("Passive")?.gameObject.SetActive(false);
                    childLocator.FindChild("Burst")?.gameObject.GetComponent<ParticleSystem>().Emit(40);
                    childLocator.FindChild("Ring")?.gameObject.GetComponent<ParticleSystem>().Emit(1);
                })
            );
        }
    }
}