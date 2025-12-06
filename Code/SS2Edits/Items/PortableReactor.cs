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
using R2API;
using SS2;
namespace LordsStarstormEdits.SS2Edits.Items;


[MonoDetourTargets(typeof(SS2.Items.PortableReactor))]
internal static class PortableReactor
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!ConfigOptions.ItemEdits.PortableReactor.Value)
        {
            return;
        }

        Mdh.SS2.Items.PortableReactor.GetStatCoefficients.Postfix(AlsoGiveArmorWhenActive);
    }

    private static void AlsoGiveArmorWhenActive(SS2.Items.PortableReactor self, ref CharacterBody sender, ref RecalculateStatsAPI.StatHookEventArgs args)
    {
        if (sender.HasBuff(SS2Content.Buffs.BuffReactor))
        {
            args.armorAdd += 100;
        }
    }

    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void ChangeTokens()
    {
        if (!ConfigOptions.ItemEdits.PortableReactor.Value)
        {
            return;
        }
        
        SS2Content.Items.PortableReactor.pickupToken = "LSE_ITEM_PORTABLEREACTOR_EDIT_PICKUP";
        SS2Content.Items.PortableReactor.descriptionToken = "LSE_ITEM_PORTABLEREACTOR_EDIT_DESC";
    }
}