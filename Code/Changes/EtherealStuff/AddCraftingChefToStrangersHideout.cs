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
namespace StarstormSquared.Changes.EtherealStuff;


[MonoDetourTargets(typeof(TeleporterUpgradeController))]
internal static class AddCraftingChefToStrangersHideout
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        SceneDirector.onPostPopulateSceneServer += OnPostPopulateSceneServer;
    }


    private static void OnPostPopulateSceneServer(SceneDirector sceneDirector)
    {
        if (!Run.instance.IsExpansionEnabled(LoadedAssets.Dlc3) || !ConfigOptions.Ethereal.AddCraftingChefToZanzanStage.Value || LoadedAssets.CraftingChef == null || SceneManager.GetActiveScene().name != "ss2_voidshop")
        {
            return;
        }


        GameObject craftingChefToSpawn = UnityEngine.Object.Instantiate<GameObject>(LoadedAssets.CraftingChef, new Vector3(-22.3698f, -0.453f, 70.9284f), Quaternion.Euler(353.67f, 80, 0));
        NetworkServer.Spawn(craftingChefToSpawn);
    }
}