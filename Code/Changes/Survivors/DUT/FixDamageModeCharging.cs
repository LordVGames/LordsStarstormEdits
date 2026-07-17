using MonoDetour;
using MonoDetour.HookGen;
using R2API;
using RoR2;
using RoR2.Orbs;
using SS2;
using SS2.Orbs;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.DUT;


[MonoDetourTargets(typeof(HealthComponent))]
internal static class FixDamageModeCharging
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        Mdh.RoR2.HealthComponent.TakeDamageProcess.Postfix(OnTakeDamage);
    }


    private static void OnTakeDamage(HealthComponent self, ref DamageInfo damageInfo)
    {
        // stupid shit???
        if (SS2.Survivors.DUT.DUTDamageType == 0)
        {
            return;
        }
        if (!damageInfo.damageType.HasModdedDamageType(SS2.Survivors.DUT.DUTDamageType))
        {
            return;
        }
        if (damageInfo.attacker == null || damageInfo.procCoefficient == 0 || !damageInfo.damageType.IsDamageSourceSkillBased)
        {
            return;
        }


        DUTRedOrb redOrb = new()
        {
            origin = damageInfo.position,
            target = damageInfo.attacker.gameObject.GetComponent<CharacterBody>().mainHurtBox
        };
        OrbManager.instance.AddOrb(redOrb); // sapping self gives 3 so 1 from any enemy is balanced ig?
    }
}