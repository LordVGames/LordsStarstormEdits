using System;
using UnityEngine;
using RoR2;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2.Orbs;
using System.Runtime.CompilerServices;
namespace StarstormSquared.Changes.Items;

[MonoDetourTargets(typeof(SS2.Items.ArmedBackpack.Behavior))]
internal static class ArmedBackpack
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        Mdh.SS2.Items.ArmedBackpack.Behavior.OnTakeDamageServer.ILHook(ReplaceMissileWithOrb);
    }

    private static void ReplaceMissileWithOrb(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipFireMissile = w.DefineLabel();


        // going to after line:
        // float num3 = victimBody.damage * num2;
        w.MatchRelaxed(
            x => x.MatchLdloc(2),
            x => x.MatchMul(),
            x => x.MatchStloc(3) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc_3), // calculated missile damage
            w.Create(OpCodes.Ldarg_1), // DamageReport
            w.CreateCall(FireMissileOrbIfApplicable),
            w.Create(OpCodes.Brtrue, skipFireMissile)
        );


        // going to before line:
        // MissileUtils.FireMissile
        w.MatchRelaxed(
            x => x.MatchCall("RoR2.MissileUtils", "FireMissile") && w.SetCurrentTo(x),
            x => x.MatchRet()
        ).ThrowIfFailure()
        .MarkLabelToCurrentNext(skipFireMissile);
    }

    private static bool FireMissileOrbIfApplicable(float missileDamage, DamageReport damageReport)
    {
        if (!ConfigOptions.ItemChanges.ArmedBackpack.Value)
        {
            return false;
        }
        if (damageReport.victimBody == null || damageReport.attackerBody == null || damageReport.victimTeamIndex != TeamIndex.Player)
        {
            return false;
        }


        MicroMissileOrb missileOrb = new()
        {
            origin = damageReport.victimBody.aimOrigin,
            damageValue = missileDamage,
            isCrit = damageReport.damageInfo.crit,
            teamIndex = damageReport.victimBody.teamComponent.teamIndex,
            attacker = damageReport.victimBody.gameObject,
            procChainMask = damageReport.damageInfo.procChainMask,
            procCoefficient = 1f,
            damageColorIndex = DamageColorIndex.Item,
            target = damageReport.attackerBody.mainHurtBox
        };
        

        if (damageReport.victimBody.inventory?.GetItemCountEffective(DLC1Content.Items.MoreMissile) > 0)
        {
            if (ModSoftDependencies.LordsItemEditsMod.ModIsRunning)
            {
                missileOrb.damageValue *= ModSoftDependencies.LordsItemEditsMod.GetEditedICBMDamageMult(damageReport.victimBody);
            }
            else
            {
                OrbManager.instance.AddOrb(missileOrb);
                OrbManager.instance.AddOrb(missileOrb);
                // gotta be authentic with the missile spam experience lmao
                Util.PlaySound("Play_item_proc_missile_fire", damageReport.attackerBody.gameObject);
                Util.PlaySound("Play_item_proc_missile_fire", damageReport.attackerBody.gameObject);
            }
        }
        OrbManager.instance.AddOrb(missileOrb);
        // the orb doesn't play a sound on fire and editing the assets isn't working so
        Util.PlaySound("Play_item_proc_missile_fire", damageReport.attackerBody.gameObject);

        
        return true;
    }
}