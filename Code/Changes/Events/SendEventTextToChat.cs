using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MSU;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events;


[MonoDetourTargets(typeof(GameplayEventTextController), GenerateControlFlowVariants = true)]
internal static class SendEventTextToChat
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.MSU.GameplayEventTextController.DequeueAndInitializeRequest.ControlFlowPrefix(SendToChatInstead);
    }


    private static ReturnFlow SendToChatInstead(GameplayEventTextController self)
    {
        if (ConfigOptions.Events.SendMessagesToChatInstead.Value)
        {
            GameplayEventTextController.EventTextRequest currentTextRequest = self._textRequests.Dequeue();
            string formatted = Language.GetStringFormatted(currentTextRequest.tokenValue);
            string coloredMessage = Util.GenerateColoredString(formatted, currentTextRequest.eventColor);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = coloredMessage
            });
            return ReturnFlow.SkipOriginal;
        }
        return ReturnFlow.None;
    }
}