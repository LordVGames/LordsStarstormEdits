/*using EntityStates.DUT;
using MonoDetour;
using MonoDetour.HookGen;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.DUT;


[MonoDetourTargets]
internal static class PlayShotUpgradeSoundsOnCharge
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.EntityStates.DUT.ChargeDamage.Charge.Postfix(PlaySoundsOnEnoughCharge);
    }


    private static void PlaySoundsOnEnoughCharge(ChargeDamage self)
    {
        // stupid shit doesnt work why
        // even with soundfixer
        RoR2.Util.PlaySound(EntityStates.BrotherMonster.EnterSkyLeap.soundString, self.controller.characterBody.gameObject);
    }
}*/