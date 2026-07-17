// TODO add the passive back myself eventually

/*using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Components;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Skills;
namespace StarstormSquared.Changes.Survivors.Knight;


[MonoDetourTargets(typeof(KnightPassiveController), GenerateControlFlowVariants = true)]
internal static class FixKnightPassive
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Knight.AddKnightPassive.Value || !SS2Config.enableBeta.value)
        {
            return;
        }

        Mdh.SS2.Components.KnightPassiveController.Start.ControlFlowPrefix(AddTheCoolPassive);
    }


    private static ReturnFlow AddTheCoolPassive(KnightPassiveController self)
    {
        self.PassiveWardPrefab = SS2Assets.LoadAsset<GameObject>("KnightPassiveBuffWard", SS2Bundle.Indev);
        if (!self.TryGetComponent<CharacterBody>(out CharacterBody characterBody))
        {
            Log.Error("Couldn't get knight's CharacterBody?");
            return ReturnFlow.None;
        }
        self.characterBody = characterBody;
        if (NetworkServer.active)
        {
            GameObject passiveWardInstance = UnityEngine.Object.Instantiate(self.PassiveWardPrefab, self.characterBody.footPosition, Quaternion.identity);
            if (
                !passiveWardInstance.TryGetComponent<TeamFilter>(out TeamFilter teamFilter)
                || !passiveWardInstance.TryGetComponent<NetworkedBodyAttachment>(out NetworkedBodyAttachment networkedBodyAttachment)
            )
            {
                return ReturnFlow.None;
            }


            teamFilter.teamIndex = self.characterBody.teamComponent.teamIndex;
            networkedBodyAttachment.AttachToGameObjectAndSpawn(self.characterBody.gameObject);
        }

        return ReturnFlow.SkipOriginal;
    }


    [SystemInitializer(dependencies: typeof(SurvivorCatalog))]
    internal static void AddKnightPassiveToBody()
    {
        if (!SS2Config.enableBeta)
        {
            return;
        }
        if (!ConfigOptions.Knight.AddKnightPassive.Value)
        {
            return;
        }
        if (!SS2Content.Survivors.survivorKnight.bodyPrefab.TryGetComponent<SkillLocator>(out SkillLocator knightSkillLocator))
        {
            return;
        }


        knightSkillLocator.passiveSkill.enabled = true;
        knightSkillLocator.passiveSkill.skillNameToken = "SS2_KNIGHT_PASSIVE_NAME";
        knightSkillLocator.passiveSkill.skillDescriptionToken = "SS2_KNIGHT_PASSIVE_DESC";
        knightSkillLocator.passiveSkill.icon = SS2Assets.FindAsset<Sprite>("texIconKnightPassiveAura");
        SS2Content.Survivors.survivorKnight.bodyPrefab.AddComponent<KnightPassiveController>();
    }


    [SystemInitializer(dependencies: typeof(BuffCatalog))]
    internal static void RecolorKnightPassiveBuff()
    {
        if (!ConfigOptions.Knight.AddKnightPassive.Value || !SS2Config.enableBeta.value)
        {
            return;
        }
        
        SS2Content.Buffs.bdKnightBuff.buffColor = SS2Content.Survivors.survivorKnight.primaryColor;
    }
}*/