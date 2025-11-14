using MonoDetour;
using SS2;
using SS2.Items;
using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil.Cil;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using R2API;

namespace LordsStarstormEdits.SS2Edits.EtherealStuff
{
    [MonoDetourTargets(typeof(AffixUltra), GenerateControlFlowVariants = true)]
    internal static class UltraWardBuffTweak
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.TweakUltraWardBuff.Value || !SS2Config.enableBeta.value)
            {
                return;
            }

            MonoDetourHooks.SS2.Items.AffixUltra.RecalculateStatsAPI_GetStatCoefficients.ControlFlowPrefix(DoTweakedUltraWardEffect);
        }

        private static ReturnFlow DoTweakedUltraWardEffect(AffixUltra self, ref CharacterBody body, ref RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body.HasBuff(SS2Content.Buffs.bdUltra))
            {
                args.attackSpeedReductionMultAdd += 0.25f;
            }
            if (body.HasBuff(SS2Content.Buffs.bdUltraBuff))
            {
                args.moveSpeedMultAdd += 0.2f;
                args.damageMultAdd += 0.1f;
                if (!body.HasBuff(SS2Content.Buffs.bdUltra))
                {
                    // not 100% sure about keeping this but ehhhhh it makes things more interesting
                    if (ConfigOptions.AddHealingToUltraWardBuff.Value)
                    {
                        args.baseRegenAdd += body.maxHealth *= 0.025f;
                    }
                }
            }

            return ReturnFlow.SkipOriginal;
        }
    }
}