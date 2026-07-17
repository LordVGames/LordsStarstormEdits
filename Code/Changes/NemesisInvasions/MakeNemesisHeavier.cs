using MonoDetour;
using MonoDetour.HookGen;
using MSU;
using R2API;
using RoR2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.NemesisInvasions;


[MonoDetourTargets(typeof(NemesisBossHelper))]
internal static class MakeNemesisHeavier
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Items.NemesisBossHelper.RecalculateStatsAPI_GetStatCoefficients.Postfix(MakeThemHeavier);
    }


    private static void MakeThemHeavier(NemesisBossHelper self, ref CharacterBody sender, ref RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (!sender.HasItem(self.ItemDef) || !ConfigOptions.NemesisInvasions.TakeLessKnockback.Value)
        {
            return;
        }


        if (sender.characterMotor) sender.characterMotor.mass = 1050f;
        if (sender.rigidbody) sender.rigidbody.mass = 1050f;
    }
}