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


internal static class FixPinkTextures
{
    internal static void FixWaterTexture(Transform gameplayObjectsHolder)
    {
        Transform water = gameplayObjectsHolder.Find("slatemines4.1/slateminesWaterNew");
        if (water == null || !water.TryGetComponent<Renderer>(out Renderer waterRenderer))
        {
            return;
        }


        if (Run.instance != null && Run.instance.IsExpansionEnabled(LoadedAssets.Dlc1))
        {
            waterRenderer.material = LoadedAssets.VoidGauntletWaterMat;
        }
        else
        {
            waterRenderer.material = LoadedAssets.Stage5TeleWaterMat;
        }
    }


    // TODO only works when renderer is loaded via unity explorer?
    internal static void FixFeathersTexture(Transform shipsObjectsHolder)
    {
        Renderer workingRenderer = shipsObjectsHolder.Find("SMShip3").GetComponent<Renderer>();
        shipsObjectsHolder.Find("SMShip1").TryGetComponent<Renderer>(out Renderer ship1Renderer);
        ship1Renderer.material = workingRenderer.material;
        shipsObjectsHolder.Find("SMShip2").TryGetComponent<Renderer>(out Renderer ship2Renderer);
        ship2Renderer.material = workingRenderer.material;
    }


    // TODO only works when renderer is loaded via unity explorer?
    internal static void FixSolusLampTexture(Transform lampsObjectsHolder)
    {
        Transform lamp8 = lampsObjectsHolder.Find("SMLamp (8)");
        if (lamp8 == null || !lamp8.TryGetComponent<Renderer>(out Renderer lampRenderer))
        {
            return;
        }


        lampRenderer.material = LoadedAssets.SmLampMat;
    }


    internal static void FixRampTexture(Transform gameplayObjectsHolder)
    {
        Transform cube = gameplayObjectsHolder.Find("Cube");
        if (cube == null || !cube.TryGetComponent<Renderer>(out Renderer cubeRenderer))
        {
            return;
        }
        Transform cube1 = gameplayObjectsHolder.Find("Cube (1)");
        if (cube1 == null || !cube1.TryGetComponent<Renderer>(out Renderer cube1Renderer))
        {
            return;
        }


        cubeRenderer.material = cube1Renderer.material;
    }
}