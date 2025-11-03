using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using SS2;
using UnityEngine;

namespace LordsStarstormEdits.SS2Edits.Survivors
{
    internal static class SurvivorIcons
    {
        // really just for knight and cyborg
        [SystemInitializer(dependencies: typeof(SurvivorCatalog))]
        internal static void ChangeSurvivorIcons()
        {
            SS2Content.Survivors.survivorKnight.bodyPrefab.GetComponent<CharacterBody>().portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconKnight");
            // why doesn't cyborg have a survivordef in ss2content yet? i mean ik he's unfinished but so is knight
            SurvivorCatalog.FindSurvivorDef("survivorCyborg2").bodyPrefab.GetComponent<CharacterBody>().portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconCyborg");
        }
    }
}