using MiscFixes.Modules;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using StarstormSquared.Changes.Stages.SlateMines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Stages.VoidShop;


[MonoDetourTargets]
internal static class DisablePinkGrassOnConfig
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }


    internal static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "ss2_voidshop")
        {
            return;
        }


        foreach (var gameObject in scene.GetRootGameObjects())
        {
            if (gameObject.name.Contains("FOLIAGE"))
            {
                gameObject.transform.Find("Grass").gameObject.SetActive(ConfigOptions.Stages.VoidShop.TogglePinkGrass.Value);
                return;
            }
        }
    }
}