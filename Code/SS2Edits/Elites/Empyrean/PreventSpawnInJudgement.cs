using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2;
using SS2.Components;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.SceneManagement;
using RoR2;
namespace StarstormSquared.SS2Edits.Elites.Empyrean;


[MonoDetourTargets(typeof(SS2.Components.Empyrean))]
internal static class PreventSpawnInJudgement
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (ConfigOptions.Elites.Empyrean.AllowEmpyreansInJudgement.Value)
        {
            return;
        }

        Mdh.SS2.Components.Empyrean.IsAvailable.ControlFlowPrefix(DoJudgementStageCheck);
    }

    private static ReturnFlow DoJudgementStageCheck(SS2.Components.Empyrean self, ref bool returnValue)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        returnValue = Run.instance.stageClearCount > 7 && sceneName != "enemiesreturns_outoftime";

        return ReturnFlow.SkipOriginal;
    }
}