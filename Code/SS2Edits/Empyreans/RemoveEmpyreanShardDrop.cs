using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LordsStarstormEdits.SS2Edits.Empyreans
{
    [MonoDetourTargets(typeof(SS2.Components.Empyrean))]
    internal static class RemoveEmpyreanShardDrop
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.RemoveEmpyreanShardDrop.Value || !SS2Config.enableBeta.value)
            {
                return;
            }

            MonoDetourHooks.SS2.Components.Empyrean.MakeElite.ILHook(SkipEmpyreanShardDrop);
        }

        private static void SkipEmpyreanShardDrop(ILManipulationInfo info)
        {
            ILWeaver w = new(info);
            ILLabel skipShardDrop = w.DefineLabel();

            w.MatchRelaxed(
                x => x.MatchLdarg(1) && w.SetCurrentTo(x),
                x => x.MatchCallvirt<Component>("get_gameObject"),
                x => x.MatchCallvirt<GameObject>("AddComponent")
            ).ThrowIfFailure();
            w.InsertBeforeCurrent(
                w.Create(OpCodes.Br, skipShardDrop)
            );

            w.MatchRelaxed(
                x => x.MatchLdarg(1) && w.SetCurrentTo(x),
                x => x.MatchLdloca(4),
                x => x.MatchCallvirt<Component>("TryGetComponent")
            ).ThrowIfFailure();
            w.MarkLabelToCurrent(skipShardDrop);
        }
    }
}