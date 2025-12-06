using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using SS2;
using UnityEngine;
namespace LordsStarstormEdits.SS2Edits.Survivors;


internal static class SurvivorIcons
{
    [SystemInitializer(dependencies: typeof(SurvivorCatalog))]
    internal static void ChangeSurvivorIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }

        SS2Content.Survivors.survivorKnight.bodyPrefab.GetComponent<CharacterBody>().portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconKnight");
        // cyborg doesn't have a survivordef in ss2content yet
        SurvivorCatalog.FindSurvivorDef("survivorCyborg2").bodyPrefab.GetComponent<CharacterBody>().portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconCyborg");
    }
}