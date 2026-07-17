using MiscFixes.Modules;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Stages.SlateMines;


internal static class FixPinkWater
{
    internal static void FixWaterTexture(Transform gameplayObjectsHolder)
    {
        if (!ConfigOptions.Stages.SlateMines.FixPinkWater.Value)
        {
            return;
        }
        Transform water = gameplayObjectsHolder.Find("slatemines4.1/slateminesWaterNew");
        if (water == null || !water.TryGetComponent<Renderer>(out Renderer waterRenderer))
        {
            return;
        }


        if (Run.instance != null)
        {
            waterRenderer.material = LoadedAssets.VoidGauntletWaterMat;
        }
    }
}