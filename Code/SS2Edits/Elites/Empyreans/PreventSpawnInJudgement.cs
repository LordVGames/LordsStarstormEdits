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

namespace LordsStarstormEdits.SS2Edits.Elites.Empyreans
{
    internal static class PreventSpawnInJudgement
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (ConfigOptions.AllowEmpyreansInJudgement.Value)
            {
                return;
            }

            MonoDetourHooks.SS2.Components.Empyrean.IsAvailable.ControlFlowPrefix(DoJudgementStageCheck);
        }

        private static ReturnFlow DoJudgementStageCheck(Empyrean self, ref bool returnValue)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            returnValue = Run.instance.stageClearCount > 7 && sceneName != "enemiesreturns_outoftime";

            return ReturnFlow.SkipOriginal;
        }
    }
}