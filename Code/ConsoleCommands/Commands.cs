using BepInEx;
using DebugToolkit;
using EnemiesReturns.Components;
using KinematicCharacterController;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using RoR2.Navigation;
using SS2;
using SS2.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[assembly: HG.Reflection.SearchableAttribute.OptInAttribute]
namespace StarstormSquared.ConsoleCommands;


[MonoDetourTargets]
internal static class Commands
{
    [ConCommand(commandName = "start_elite_event", flags = ConVarFlags.None, helpText = "Manually starts an elite event. Must be used on a stage with natural spawns or there will be an error!\nExisting types: blazing, overloading, glacial, mending")]
    private static void StartEliteEvent(ConCommandArgs args)
    {
        if (EventDirector.instance == null)
        {
            Log.Error("SS2 event director is null, cannot use command at this time.");
            return;
        }
        string eliteEventType = args.GetArgString(0);
        GameObject chosenEliteEvent = null;


        switch (eliteEventType)
        {
            case "blazing":
                chosenEliteEvent = SS2Assets.LoadAsset<GameObject>("BlazingEliteEventController", SS2Bundle.All);
                if (chosenEliteEvent == null)
                {
                    Log.Error("Can't start blazing elite event because the event is null!");
                    return;
                }

                break;
            case "overloading":
                chosenEliteEvent = SS2Assets.LoadAsset<GameObject>("OverloadingEliteEventController", SS2Bundle.All);
                if (chosenEliteEvent == null)
                {
                    Log.Error("Can't start overloading elite event because the event is null!");
                    return;
                }

                break;
            case "glacial":
                chosenEliteEvent = SS2Assets.LoadAsset<GameObject>("GlacialEliteEventController", SS2Bundle.All);
                if (chosenEliteEvent == null)
                {
                    Log.Error("Can't start glacial elite event because the event is null!");
                    return;
                }

                break;
            case "mending":
                chosenEliteEvent = SS2Assets.LoadAsset<GameObject>("MendingEliteEventController", SS2Bundle.All);
                if (Run.instance != null && !Run.instance.IsExpansionEnabled(LoadedAssets.Dlc1))
                {
                    Log.Error("Can't start mending elite event because the Survivors of the Void expansion is not enabled!");
                    return;
                }
                if (chosenEliteEvent == null)
                {
                    Log.Error("Can't start mending elite event because the event is null!");
                    return;
                }

                break;
            default:
                Log.Error("Can't start elite event, an invalid elite type was provided!");
                break;
        }


        EventDirector.instance.StartEvent(chosenEliteEvent);
    }
}