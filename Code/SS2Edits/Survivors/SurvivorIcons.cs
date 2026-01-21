using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using SS2;
using UnityEngine;
namespace StarstormSquared.SS2Edits.Survivors;


internal static class SurvivorIcons
{
    internal static bool SetIcons = false;


    internal static void ChangeSurvivorIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }
        if (!MyAssets.Initialized || MyAssets.SurvivorIcons.AssetBundle == null)
        {
            Log.Warning("Assets are somehow not initialized yet when trying to apply new survivor icons. Trying to initialize again...");
            MyAssets.Init();
            ChangeSurvivorIcons();
            return;
        }


        if (SS2Content.Survivors.survivorKnight == null || SS2Content.Survivors.survivorKnight.bodyPrefab == null || !SS2Content.Survivors.survivorKnight.bodyPrefab.TryGetComponent<CharacterBody>(out CharacterBody knightCharacterBody) || knightCharacterBody == null)
        {
            Log.Warning("KNIGHT SURVIVOR COULD NOT BE FOUND BUT IT PROBABLY WORKED ANYWAYS FOR SOME REASON");
            return;
        }
        knightCharacterBody.portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconKnight");



        // cyborg doesn't have a survivordef in ss2content yet
        SurvivorDef cyborgSurvivorDef = SurvivorCatalog.FindSurvivorDef("survivorCyborg2");
        if (cyborgSurvivorDef == null || cyborgSurvivorDef.bodyPrefab == null || !cyborgSurvivorDef.bodyPrefab.TryGetComponent<CharacterBody>(out CharacterBody cyborgCharacterBody) || cyborgCharacterBody == null)
        {
            Log.Error("CYBORG SURVIVOR COULD NOT BE FOUND TO GIVE NEW ICON TO???");
            return;
        }
        cyborgCharacterBody.portraitIcon = MyAssets.SurvivorIcons.AssetBundle.LoadAsset<Texture>("texIconCyborg");


        SetIcons = true;
    }
}