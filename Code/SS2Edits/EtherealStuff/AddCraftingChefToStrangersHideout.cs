using System;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using RoR2.ContentManagement;
using RoR2.ExpansionManagement;
using SS2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
namespace StarstormSquared.SS2Edits.EtherealStuff;


[MonoDetourTargets(typeof(TeleporterUpgradeController))]
internal static class AddCraftingChefToStrangersHideout
{
    private static readonly AssetReferenceT<ExpansionDef> _dlc3AssetReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3.DLC3_asset);
    private static ExpansionDef _dlc3;
    private static readonly AssetReferenceT<GameObject> _craftingChefAssetReference = new(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC3_MealPrep.MealPrep_prefab);
    private static GameObject _craftingChef;


    [MonoDetourHookInitialize]
    private static void Setup()
    {
        AssetAsyncReferenceManager<ExpansionDef>.LoadAsset(_dlc3AssetReference).Completed += (handle) =>
        {
            _dlc3 = handle.Result;
        };
        AssetAsyncReferenceManager<GameObject>.LoadAsset(_craftingChefAssetReference).Completed += (handle) =>
        {
            _craftingChef = handle.Result;
        };


        SceneDirector.onPostPopulateSceneServer += OnPostPopulateSceneServer;
    }


    private static void OnPostPopulateSceneServer(SceneDirector sceneDirector)
    {
        if (!Run.instance.IsExpansionEnabled(_dlc3) || !ConfigOptions.Ethereal.AddCraftingChefToZanzanStage.Value || _craftingChef == null || SceneManager.GetActiveScene().name != "ss2_voidshop")
        {
            return;
        }


        GameObject craftingChefToSpawn = UnityEngine.Object.Instantiate<GameObject>(_craftingChef, new Vector3(-22.3698f, -0.453f, 70.9284f), Quaternion.Euler(353.67f, 80, 0));
        NetworkServer.Spawn(craftingChefToSpawn);
    }
}