using System;
using UnityEngine;
using RoR2;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2.Orbs;
using System.Runtime.CompilerServices;
using R2API;
namespace StarstormSquared.SS2Edits.Survivors.Cyborg;


[MonoDetourTargets(typeof(EntityStates.Cyborg2.Unmaker))]
internal static class DefaultPrimaryDamageBuff
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        EntityStates.Cyborg2.Unmaker.damageCoefficient = 3.2f;
    }
}