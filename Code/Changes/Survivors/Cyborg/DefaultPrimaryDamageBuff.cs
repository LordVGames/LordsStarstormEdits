using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Orbs;
using SS2;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace StarstormSquared.Changes.Survivors.Cyborg;


[MonoDetourTargets(typeof(EntityStates.Cyborg2.Unmaker))]
internal static class DefaultPrimaryDamageBuff
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }


        EntityStates.Cyborg2.Unmaker.damageCoefficient = 3.2f;
    }
}