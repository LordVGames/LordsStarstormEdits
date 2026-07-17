using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using SS2.Components;
using StarstormSquared.Changes.Stages.SlateMines;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Interactables.Zanzan;


[MonoDetourTargets]
internal static class PleaseBeQuietZanzan
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.TraderController.Awake.Postfix(AfterAwake);
    }


    private static void AfterAwake(TraderController self)
    {
        if (self.gameObject.name == "TraderBody(Clone)" && self.gameObject.TryGetComponent<SfxLocator>(out var sfxLocator))
        {
            sfxLocator.barkSound = "";
        }
    }
}