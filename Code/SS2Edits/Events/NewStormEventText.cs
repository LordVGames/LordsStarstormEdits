using MiscFixes.Modules;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using MSU;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace LordsStarstormEdits.SS2Edits.Events;


[MonoDetourTargets(typeof(EntityStates.Events.Storm))]
internal static class NewStormEventText
{
    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!ConfigOptions.Events.NewStormEventText.Value || !Storm.ReworkedStorm.value)
        {
            return;
        }

        Mdh.EntityStates.Events.Storm.OnEnter.ILHook(ChangeStormEventText);
        Mdh.EntityStates.Events.Storm.FixedUpdate.ILHook(ChangeStormEventEndText);
    }


    private static void ChangeStormEventText(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        w.MatchRelaxed(
            x => x.MatchLdloc(1),
            x => x.MatchStloc(2) && w.SetCurrentTo(x)
        ).ThrowIfFailure();
        w.InsertAfterCurrent(
            w.Create(OpCodes.Ldarg_0),
            w.Create(OpCodes.Ldloc_2),
            w.CreateCall(GetCorrectStormTextToken),
            w.Create(OpCodes.Stloc_2)
        );
    }
    private static GameplayEventTextController.EventTextRequest GetCorrectStormTextToken(EntityStates.Events.Storm storm, GameplayEventTextController.EventTextRequest eventTextRequest)
    {
        string eventTextToken;
        switch (storm.stormLevel)
        {
            case 1:
                eventTextToken = "SS2_EVENT_THUNDERSTORM_START";
                break;
            case 2:
                eventTextToken = "LSE_STORM_LEVEL_2";
                break;
            case 3:
                eventTextToken = "LSE_STORM_LEVEL_3";
                break;
            case 4:
                eventTextToken = "LSE_STORM_LEVEL_4";
                break;
            default:
                eventTextToken = Language.GetStringFormatted("LSE_STORM_LEVEL_OTHER", storm.stormLevel);
                break;
        }
        eventTextRequest.eventToken = eventTextToken;
        return eventTextRequest;
    }


    private static void ChangeStormEventEndText(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        w.MatchRelaxed(
            x => x.MatchLdstr("ermmmm..... bye storm") && w.SetCurrentTo(x)
        ).ThrowIfFailure();
        w.Current.Operand = "SS2_EVENT_THUNDERSTORM_END";
    }
}