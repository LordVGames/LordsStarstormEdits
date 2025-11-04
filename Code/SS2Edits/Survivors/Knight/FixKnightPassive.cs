using Mono.Cecil.Cil;
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

namespace LordsStarstormEdits.SS2Edits.Survivors.Knight
{
    [MonoDetourTargets(typeof(KnightPassiveController), GenerateControlFlowVariants = true)]
    internal static class FixKnightPassive
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.AddKnightPassive.Value || !SS2Config.enableBeta.value)
            {
                return;
            }

            MonoDetourHooks.SS2.Components.KnightPassiveController.Start.ControlFlowPrefix(AddTheCoolPassive);
        }


        private static ReturnFlow AddTheCoolPassive(KnightPassiveController self)
        {
            self.PassiveWardPrefab = SS2Assets.LoadAsset<GameObject>("KnightPassiveBuffWard", SS2Bundle.Indev);
            self.characterBody = self.transform.GetComponent<CharacterBody>();
            if (NetworkServer.active)
            {
                GameObject passiveWardInstance = UnityEngine.Object.Instantiate(self.PassiveWardPrefab, self.characterBody.footPosition, Quaternion.identity);
                passiveWardInstance.GetComponent<TeamFilter>().teamIndex = self.characterBody.teamComponent.teamIndex;
                passiveWardInstance.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(self.characterBody.gameObject);
            }

            return ReturnFlow.SkipOriginal;
        }


        [SystemInitializer(dependencies: typeof(SurvivorCatalog))]
        internal static void AddKnightPassiveToBody()
        {
            if (!ConfigOptions.AddKnightPassive.Value || !SS2Config.enableBeta.value)
            {
                return;
            }

            SS2Content.Survivors.survivorKnight.bodyPrefab.AddComponent<KnightPassiveController>();

            SkillLocator knightSkillLocator = SS2Content.Survivors.survivorKnight.bodyPrefab.GetComponent<SkillLocator>();
            knightSkillLocator.passiveSkill.enabled = true;
            knightSkillLocator.passiveSkill.skillNameToken = "SS2_KNIGHT_PASSIVE_NAME";
            knightSkillLocator.passiveSkill.skillDescriptionToken = "SS2_KNIGHT_PASSIVE_DESC";
            knightSkillLocator.passiveSkill.icon = SS2Assets.FindAsset<Sprite>("texIconKnightPassiveAura");
        }


        [SystemInitializer(dependencies: typeof(BuffCatalog))]
        internal static void RecolorKnightPassiveBuff()
        {
            if (!ConfigOptions.AddKnightPassive.Value || !SS2Config.enableBeta.value)
            {
                return;
            }
            
            SS2Content.Buffs.bdKnightBuff.buffColor = SS2Content.Survivors.survivorKnight.primaryColor;
        }
    }
}