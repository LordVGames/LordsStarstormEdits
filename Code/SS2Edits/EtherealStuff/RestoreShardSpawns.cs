using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Items;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;

namespace LordsStarstormEdits.SS2Edits.EtherealStuff
{
    internal static class RestoreShardSpawns
    {
        [MonoDetourTargets(typeof(ShardGold))]
        internal static class RestoreGoldShardDrop
        {
            internal static ShardGold shardGoldInstance;

            [MonoDetourHookInitialize]
            internal static void Setup()
            {
                if (!ConfigOptions.RestoreGoldShardDrop.Value)
                {
                    return;
                }

                MonoDetourHooks.SS2.Items.ShardGold.Initialize.Postfix(AddCommentedOutHook);
            }

            private static void AddCommentedOutHook(ShardGold self)
            {
                shardGoldInstance = self;
                // would use a monodetour version of the on hooks but then i couldn't use the existing ss2 method since monodetour hooks don't come with an orig
                On.RoR2.HalcyoniteShrineInteractable.DropRewards += YouWillDropGoldShards;
            }

            private static void YouWillDropGoldShards(On.RoR2.HalcyoniteShrineInteractable.orig_DropRewards orig, RoR2.HalcyoniteShrineInteractable self)
            {
                MonoDetourHooks.SS2.Items.ShardGold.SpawnGoldShard.Target().Invoke(shardGoldInstance, [orig, self]);
                orig(self);
            }
        }



        [MonoDetourTargets(typeof(ShardVoid))]
        internal static class RestoreVoidShardDrop
        {
            internal static ShardVoid shardVoidInstance;

            [MonoDetourHookInitialize]
            internal static void Setup()
            {
                if (!ConfigOptions.RestoreVoidShardDrop.Value)
                {
                    return;
                }

                MonoDetourHooks.SS2.Items.ShardVoid.Initialize.Postfix(AddCommentedOutHook);
            }

            private static void AddCommentedOutHook(ShardVoid self)
            {
                shardVoidInstance = self;
                On.EntityStates.VoidCamp.Deactivate.OnEnter += YouWillDropVoidShards;
            }

            private static void YouWillDropVoidShards(On.EntityStates.VoidCamp.Deactivate.orig_OnEnter orig, EntityStates.VoidCamp.Deactivate self)
            {
                MonoDetourHooks.SS2.Items.ShardVoid.SpawnVoidShard.Target().Invoke(shardVoidInstance, [orig, self]);
                orig(self);
            }
        }


        // TODO they removed the item entirely from the current build. idk how to add items yet
        // i'll be back for this unless they re-add it before i do
        /*[MonoDetourTargets(typeof(EntityStates.Events.Storm))]
        internal static class RestoreStormShardDrop
        {
            [MonoDetourHookInitialize]
            internal static void Setup()
            {
                if (!ConfigOptions.RestoreStormShardDrops.Value || !Storm.ReworkedStorm)
                {
                    return;
                }

                MonoDetourHooks.EntityStates.Events.Storm.OnEnter.Postfix(AddShardDropOnCompletion);
                MonoDetourHooks.EntityStates.Events.Storm.OnExit.Postfix(RemoveShardDropOnCompletion);
            }


            private static void AddShardDropOnCompletion(EntityStates.Events.Storm storm)
            {
                if (!NetworkServer.active)
                {
                    return;
                }
                CombatDirector bossDirector = TeleporterInteraction.instance?.bossDirector;
                if (bossDirector == null || storm.stormLevel < EntityStates.Events.Storm.bossEliteLevel)
                {
                    return;
                }

                BossGroup.onBossGroupDefeatedServer += OnBossGroupDefeatedServer;
            }
            private static void RemoveShardDropOnCompletion(EntityStates.Events.Storm storm)
            {
                if (!NetworkServer.active)
                {
                    return;
                }
                CombatDirector bossDirector = TeleporterInteraction.instance?.bossDirector;
                if (bossDirector == null || storm.stormLevel < EntityStates.Events.Storm.bossEliteLevel)
                {
                    return;
                }

                BossGroup.onBossGroupDefeatedServer -= OnBossGroupDefeatedServer;
            }
            private static void OnBossGroupDefeatedServer(BossGroup bossGroup)
            {
                if (bossGroup == TeleporterInteraction.instance.bossGroup && Run.instance.participatingPlayerCount > 0)
                {
                    int playerCount = Run.instance.participatingPlayerCount;
                    float angle = 360f / (float)playerCount;
                    Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
                    Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
                    PickupIndex drop = PickupCatalog.FindPickupIndex(SS2Content.Items.ShardStorm.itemIndex);
                    int i = 0;
                    while (i < playerCount)
                    {
                        PickupDropletController.CreatePickupDroplet(drop, bossGroup.dropPosition.position, vector);
                        i++;
                        vector = rotation * vector;
                    }
                }
            }
        }*/



        [MonoDetourTargets(typeof(EliteEventMissionController))]
        internal static class RestoreSuperEliteShardDrops
        {
            [MonoDetourHookInitialize]
            internal static void Setup()
            {
                if (!ConfigOptions.RestoreSuperEliteShardDrops.Value)
                {
                    return;
                }

                MonoDetourHooks.SS2.EliteEventMissionController.OnBossKilledServer.OnKilledServer.ILHook(SkipBadDropReplacement);
            }

            private static void SkipBadDropReplacement(ILManipulationInfo info)
            {
                ILWeaver w = new(info);
                ILLabel skipBadLine = w.DefineLabel();


                w.MatchRelaxed(
                    x => x.MatchLdsfld("SS2.SS2Content/Items", "ShardStorm") && w.SetCurrentTo(x),
                    x => x.MatchCallvirt(out _),
                    x => x.MatchCall(out _),
                    x => x.MatchStloc(3)
                ).ThrowIfFailure();
                w.InsertBeforeCurrent(
                    w.Create(OpCodes.Br, skipBadLine)
                );


                w.MatchRelaxed(
                    x => x.MatchLdsfld("SS2.SS2Content/Items", "ShardStorm"),
                    x => x.MatchCallvirt(out _),
                    x => x.MatchCall(out _),
                    x => x.MatchStloc(3) && w.SetCurrentTo(x)
                ).ThrowIfFailure();
                w.MarkLabelToCurrentNext(skipBadLine);
            }
        }
    }
}