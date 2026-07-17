using RoR2;
using RoR2.Skills;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Survivors.DUT;


internal static class GiveSkillIcons
{
    //[SystemInitializer(dependencies: typeof(SurvivorCatalog))]
    internal static void GiveDUTSkillIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        SurvivorDef dutSurvivor = SurvivorCatalog.FindSurvivorDef("survivorDUT");
        if (dutSurvivor == null || dutSurvivor.bodyPrefab == null || !dutSurvivor.bodyPrefab.TryGetComponent<CharacterBody>(out CharacterBody dutCharacterBody) || dutCharacterBody == null)
        {
            Log.Warning("DUT SURVIVOR COULD NOT BE FOUND TO GIVE SKILL ICONS TO");
            return;
        }
        var dutSkillLocator = dutSurvivor.bodyPrefab.GetComponent<SkillLocator>();



        dutSkillLocator.primary.skillFamily.defaultSkillDef.icon = LoadedAssets.RailgunnerShotIcon;
        dutSkillLocator.utility.skillFamily.defaultSkillDef.icon = LoadedAssets.ToolbotDashIcon;
        // mode starts in damage mode, so switch icon should show as healing to start
        dutSkillLocator.special.skillFamily.defaultSkillDef.icon = LoadedAssets.ToolbotSwapIcon;
    }
}