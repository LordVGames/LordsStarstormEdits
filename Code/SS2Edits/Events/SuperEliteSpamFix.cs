using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace LordsStarstormEdits.SS2Edits.Events
{
    [MonoDetourTargets(typeof(ExtraEquipmentManager))]
    internal static class SuperEliteSpamFix
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            MonoDetourHooks.SS2.ExtraEquipmentManager.ExtraEquipmentDisplays.EnableEquipmentDisplay.ILHook(ReturnIfDisplayRuleGroupEmpty);
            MonoDetourHooks.SS2.ExtraEquipmentManager.ExtraEquipmentDisplays.InstantiateDisplayRuleGroup.ILHook(ReturnIfItemDisplayEmpty);
        }


        private static void ReturnIfDisplayRuleGroupEmpty(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            ILLabel allowIfNotEmpty = w.DefineLabel();

            w.MatchRelaxed(
                x => x.MatchLdarg(1),
                x => x.MatchCallvirt<ItemDisplayRuleSet>("GetEquipmentDisplayRuleGroup"),
                x => x.MatchStloc(0) && w.SetCurrentTo(x)
            ).ThrowIfFailure();
            w.InsertAfterCurrent(
                w.Create(OpCodes.Ldloc_0)
            );
            w.InsertAfterCurrent(
                w.CreateDelegateCall(
                    (DisplayRuleGroup drs) =>
                    {
                        return drs.isEmpty;
                    }
                )
            );
            w.InsertAfterCurrent(
                w.Create(OpCodes.Brfalse, allowIfNotEmpty),
                w.Create(OpCodes.Ret)
            );
            w.MarkLabelToCurrentNext(allowIfNotEmpty);
        }


        private static void ReturnIfItemDisplayEmpty(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            ILLabel returnIfNull = w.DefineLabel();


            // grabbing label to exit out to
            w.MatchRelaxed(
                x => x.MatchLdloc(5),
                x => x.MatchCall<UnityEngine.Object>("op_Implicit"),
                x => x.MatchBrfalse(out returnIfNull)
            ).ThrowIfFailure();


            w.MatchRelaxed(
                x => x.MatchLdloc(7),
                x => x.MatchStloc(6) && w.SetCurrentTo(x)
            ).ThrowIfFailure();
            w.InsertAfterCurrent(
                w.Create(OpCodes.Ldloc, 6),
                w.Create(OpCodes.Brtrue, returnIfNull)
            );
            w.InsertBeforeCurrent(
                w.CreateDelegateCall(
                    (CharacterModel.ParentedPrefabDisplay parentedPrefabDisplay) =>
                    {
                        return parentedPrefabDisplay.itemDisplay == null;
                    }
                )
            );
        }
    }
}