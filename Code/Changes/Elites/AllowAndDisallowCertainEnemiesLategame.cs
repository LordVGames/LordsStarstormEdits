using System;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2.Components;
using UnityEngine.SceneManagement;
namespace StarstormSquared.Changes.Elites;


[MonoDetourTargets(typeof(CustomEliteDirector), GenerateControlFlowVariants = true)]
internal static class AllowAndDisallowCertainEnemiesLategame
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.Components.CustomEliteDirector.ModifySpawn.ILHook(CustomEliteDirector_ModifySpawn);
    }
    

    private static void CustomEliteDirector_ModifySpawn(ILManipulationInfo info)
    {
        ILWeaver w = new(info);


        ILLabel continueCodeLocation = null;
        Instruction startOfSkip = null!;
        Instruction endOfSkip = null!;
        w.MatchRelaxed(
            x => x.MatchLdarg(2) && w.SetInstructionTo(ref startOfSkip, x),
            x => x.MatchLdfld(out _),
            x => x.MatchLdfld(out _),
            x => x.MatchLdfld<SpawnCard>("eliteRules"),
            x => x.MatchBrfalse(out continueCodeLocation),
            x => x.MatchRet() && w.SetInstructionTo(ref endOfSkip, x) && w.SetCurrentTo(x)
        ).ThrowIfFailure();
        // removing intiial elite rules check because i want more enemies to be ethereal/ultra including lunar enemies
        if (ConfigOptions.Elites.AllLateGameElites.RemoveEtherealAndUltraRestriction.Value)
        {
            w.InsertBranchOver(startOfSkip, endOfSkip);
        }
        // also preventing jellyfish and larva from becoming lategame elites here because they do nothing 99% of the time
        w.InsertAfterCurrent(
            w.Create(OpCodes.Ldloc_1),
            w.CreateDelegateCall((CharacterBody body) =>
            {
                if (
                    (
                        body == RoR2Content.BodyPrefabs.JellyfishBody
                        || body == DLC1Content.BodyPrefabs.AcidLarvaBody
                    )
                    && ConfigOptions.Elites.AllLateGameElites.DisallowSelfDamagingEnemies.Value)
                {
                    return false;
                }
                return true;
            }),
            w.Create(OpCodes.Brtrue, continueCodeLocation),
            w.Create(OpCodes.Ret)
        );


        ILLabel goToNextAffix = null;
        w.MatchRelaxed(
            x => x.MatchLdloc(8),
            x => x.MatchCallOrCallvirt<StackableAffix>("IsAvailable"),
            x => x.MatchBrfalse(out goToNextAffix) && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertAfterCurrent(
            w.Create(OpCodes.Ldloc, 8),
            w.Create(OpCodes.Ldarg_2),
            w.CreateDelegateCall((StackableAffix stackableAffix, SpawnCard.SpawnResult spawnResult) =>
            {
                if (
                    stackableAffix.GetType().Name == "Empyrean"
                    && spawnResult.spawnRequest.spawnCard.eliteRules != SpawnCard.EliteRules.Default
                )
                {
                    return false;
                }
                return true;
            }),
            w.Create(OpCodes.Brfalse, goToNextAffix)
        );
    }
}