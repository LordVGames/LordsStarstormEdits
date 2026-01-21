/* using System;
using MonoDetour;
using R2API;
using RoR2;
using RoR2.ContentManagement;
using SS2;
using UnityEngine;
namespace StarstormSquared.SS2Edits.Items.Shards.ShardItemEffects;


public static class ShardFire
{
    private const float _procChance = 33f;


    [MonoDetourHookInitialize]
    private static void Setup()
    {
        DotController.onDotInflictedServerGlobal += OnDotInflictedServerGlobal;
    }


    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void ChangeTokens()
    {
        SS2Content.Items.ShardFire.pickupToken = "SS22_ITEM_SHARDFIRE_NEWEFFECT_PICKUP";
        SS2Content.Items.ShardFire.descriptionToken = "SS22_ITEM_SHARDFIRE_NEWEFFECT_DESC";
    }


    private static void OnDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo)
    {
        if (inflictDotInfo.attackerObject == null || inflictDotInfo.victimObject == null)
        {
            return;
        }
        switch (inflictDotInfo.dotIndex)
        {
            case DotController.DotIndex.Burn:
            case DotController.DotIndex.StrongerBurn:
            case DotController.DotIndex.Helfire:
                return;
        }
        Log.Warning("OnDotInflictedServerGlobal");
        CharacterBody attackerBody = inflictDotInfo.attackerObject.GetComponent<CharacterBody>();
        if (attackerBody == null || attackerBody.inventory == null || attackerBody.master == null)
        {
            return;
        }
        int shardFireCount = attackerBody.inventory.GetItemCountEffective(SS2Content.Items.ShardFire);
        if (shardFireCount < 1)
        {
            return;
        }
        CharacterBody victimBody = inflictDotInfo.victimObject.GetComponent<CharacterBody>();
        if (victimBody == null || victimBody.inventory == null)
        {
            return;
        }
        if (victimBody.HasBuff(DontReapplyBurnBuff.bdDontReapplyBurn))
        {
            return;
        }
        if (!Util.CheckRoll(_procChance, attackerBody.master))
        {
            return;
        }



        for (int i = 0; i < shardFireCount; i++)
        {
            DotController.InflictDot(ref inflictDotInfo);
        }
        // TODO does this work for clients in mp?
        victimBody.AddTimedBuff(DontReapplyBurnBuff.bdDontReapplyBurn, 0.2f);
        //Util.PlaySound("Play_item_proc_moneyOnKill_loot", characterBody.gameObject);
    }


    public static class DontReapplyBurnBuff
    {
        public static BuffDef bdDontReapplyBurn;

        internal static void SetupBuff()
        {
            bdDontReapplyBurn = ScriptableObject.CreateInstance<BuffDef>();
            bdDontReapplyBurn.name = "bdDontReapplyBurn";
            bdDontReapplyBurn.isHidden = true;
            bdDontReapplyBurn.canStack = false;
            bdDontReapplyBurn.isDebuff = false;
            bdDontReapplyBurn.flags = BuffDef.Flags.ExcludeFromNoxiousThorns;
            bdDontReapplyBurn.ignoreGrowthNectar = true;
            bdDontReapplyBurn.iconSprite = SS2Content.Items.ShardFire.pickupIconSprite;
            ContentAddition.AddBuffDef(bdDontReapplyBurn);
        }
    }
} */