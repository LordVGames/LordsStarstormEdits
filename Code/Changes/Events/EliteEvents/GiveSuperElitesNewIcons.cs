using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events.EliteEvents;


internal static class GiveSuperElitesNewIcons
{
    private static bool loadedAssets = false;
    private static Sprite blazingSuperEliteSprite;
    private static Sprite glacialSuperEliteSprite;
    private static Sprite mendingSuperEliteSprite;
    private static Sprite overloadingSuperEliteSprite;


    private static void LoadAssets()
    {
        if (loadedAssets)
        {
            return;
        }
        // ffufdjfudf
        if (MyAssets.SuperEliteIcons.AssetBundle == null)
        {
            MyAssets.Init();
        }


        blazingSuperEliteSprite = MyAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteBlazing");
        glacialSuperEliteSprite = MyAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteGlacial");
        mendingSuperEliteSprite = MyAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteMending");
        overloadingSuperEliteSprite = MyAssets.SuperEliteIcons.AssetBundle.LoadAsset<Sprite>("SuperEliteOverloading");
        loadedAssets = true;
    }


    [SystemInitializer(dependencies: typeof(BuffCatalog))]
    private static void AddBuffIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }
        LoadAssets();
        // stupidddddddddd
        if (blazingSuperEliteSprite == null)
        {
            LoadAssets();
        }


        SS2Content.Buffs.BuffAffixSuperFire.iconSprite = blazingSuperEliteSprite;
        // why is only super fire solid red??? what
        SS2Content.Buffs.BuffAffixSuperFire.buffColor = SS2Content.Buffs.BuffAffixSuperIce.buffColor;
        SS2Content.Buffs.BuffAffixSuperIce.iconSprite = glacialSuperEliteSprite;
        SS2Content.Buffs.BuffAffixSuperEarth.iconSprite = mendingSuperEliteSprite;
        SS2Content.Buffs.BuffAffixSuperLightning.iconSprite = overloadingSuperEliteSprite;
    }
    
    
    [SystemInitializer(dependencies: typeof(EquipmentCatalog))]
    private static void AddEquipmentIcons()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }
        LoadedAssets.LoadEquipments();


        // have to load the equipments this way bc the SS2Content versions are null at this point?????
        LoadedAssets.superFireEquipmentDef.pickupIconSprite = blazingSuperEliteSprite;
        LoadedAssets.superIceEquipmentDef.pickupIconSprite = glacialSuperEliteSprite;
        LoadedAssets.superEarthEquipmentDef.pickupIconSprite = mendingSuperEliteSprite;
        LoadedAssets.superLightningEquipmentDef.pickupIconSprite = overloadingSuperEliteSprite;
    }
}