using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Stages.SlateMines;


[MonoDetourTargets]
internal static class SlateMinesMain
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneDirector.onPostPopulateSceneServer += SceneDirector_onPostPopulateSceneServer;
    }

    private static void SceneDirector_onPostPopulateSceneServer(SceneDirector obj)
    {
    }

    private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name != "ss2_slatemines")
        {
            return;
        }


        foreach (var gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.name.Contains("GAMEPLAY"))
            {
                RemoveCommandosForScale.DeleteCommandos(gameObject.transform);
                continue;
            }
        }
    }
}