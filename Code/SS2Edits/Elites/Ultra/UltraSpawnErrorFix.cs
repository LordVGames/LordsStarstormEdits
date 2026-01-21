using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace StarstormSquared.SS2Edits.Elites.Ultra;


// this is mainly just to help log clutter
[MonoDetourTargets(typeof(BoostCharacterSize.BodyBehavior), GenerateControlFlowVariants = true)]
internal static class UltraSpawnErrorFix
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Items.BoostCharacterSize.BodyBehavior.ModifySkillDrivers.ControlFlowPrefix(PreventIfNotAI);
    }

    private static ReturnFlow PreventIfNotAI(BoostCharacterSize.BodyBehavior self, ref float deltaScale)
    {
        if (self.body == null || self.body.master.aiComponents.Length < 1)
        {
            return ReturnFlow.SkipOriginal;
        }
        return ReturnFlow.None;
    }
}