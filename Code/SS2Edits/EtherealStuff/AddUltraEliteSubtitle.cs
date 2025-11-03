using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace LordsStarstormEdits.SS2Edits.EtherealStuff
{
    [MonoDetourTargets(typeof(AffixUltra), GenerateControlFlowVariants = true)]
    internal static class AddUltraEliteSubtitle
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            // why would you want to keep the subtitle as "horde of many"
            if (!SS2Config.enableBeta.value)
            {
                return;
            }

            MonoDetourHooks.SS2.Items.AffixUltra.BodyBehavior.Start.Postfix(AddSS1Subtitle);
        }

        // can't really store the subtitle without messing with FixedConditionalWeakTables and i don't think it matters that much
        private static void AddSS1Subtitle(AffixUltra.BodyBehavior self)
        {
            self.body?.subtitleNameToken = "LSE_ULTRA_SUBTITLE_SS1";
            Log.Warning(self.body?.subtitleNameToken);
        }
    }
}