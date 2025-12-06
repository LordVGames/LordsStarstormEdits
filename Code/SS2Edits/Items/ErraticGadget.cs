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
using SS2;
using R2API;
[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace LordsStarstormEdits.SS2Edits.Items;


[MonoDetourTargets(typeof(SS2.Items.ErraticGadget), GenerateControlFlowVariants = true)]
internal static class ErraticGadget
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        switch(ConfigOptions.ItemEdits.ErraticGadget.Value)
        {
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.None:
                return;
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.DamageMultAndOnHitProc:
                Mdh.SS2.Items.ErraticGadget.LightningOrb_OnArrival.ILHook(SkipDoublingProc);
                Mdh.SS2.Items.ErraticGadget.LightningStrikeOrb_OnArrival.ControlFlowPrefix(LightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.SimpleLightningStrikeOrb_OnArrival.ControlFlowPrefix(SimpleLightningStrikeOrb_JustMoreDamage);
                // Polylute isn't used rn
                //Mdh.SS2.Items.ErraticGadget.VoidLightningOrb_Begin.ControlFlowPrefix(VoidLightningOrb_JustMoreDamage);
                break;
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.OnlyDamageMult:
                Mdh.SS2.Items.ErraticGadget.LightningOrb_OnArrival.ILHook(SkipDoublingProc);
                Mdh.SS2.Items.ErraticGadget.LightningStrikeOrb_OnArrival.ControlFlowPrefix(LightningStrikeOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.SimpleLightningStrikeOrb_OnArrival.ControlFlowPrefix(SimpleLightningStrikeOrb_JustMoreDamage);
                //Mdh.SS2.Items.ErraticGadget.VoidLightningOrb_Begin.ControlFlowPrefix(VoidLightningOrb_JustMoreDamage);
                Mdh.SS2.Items.ErraticGadget.Behavior.OnDamageDealtServer.ControlFlowPrefix(ErraticGadget_OnDamageDealtServer);
                break;
        }
    }


    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void ChangeTokens()
    {
        switch (ConfigOptions.ItemEdits.ErraticGadget.Value)
        {
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.None:
                return;
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.DamageMultAndOnHitProc:
                SS2Content.Items.ErraticGadget.pickupToken = "LSE_ITEM_ERRATICGADGET_EDIT1_PICKUP";
                SS2Content.Items.ErraticGadget.descriptionToken = "LSE_ITEM_ERRATICGADGET_EDIT1_DESC";
                break;
            case ConfigOptions.ItemEdits.ErraticGadgetEditType.OnlyDamageMult:
                SS2Content.Items.ErraticGadget.pickupToken = "LSE_ITEM_ERRATICGADGET_EDIT2_PICKUP";
                SS2Content.Items.ErraticGadget.descriptionToken = "LSE_ITEM_ERRATICGADGET_EDIT2_DESC";
                break;
        }
    }


    private static void SkipDoublingProc(ILManipulationInfo info)
    {
        ILWeaver w = new(info);
        ILLabel skipDoublingProc = w.DefineLabel();


        // next 2 blocks are to skip over the part where lightning is doubled
        // going to before "bool flag = false;"
        w.MatchRelaxed(
            x => x.MatchLdcI4(0) && w.SetCurrentTo(x),
            x => x.MatchStloc(0)
        ).ThrowIfFailure();
        w.InsertBeforeCurrentStealLabels(
            w.Create(OpCodes.Br, skipDoublingProc)
        );


        // going to before:
        // orig.Invoke(self);
        // by matching after this line:
        // self.bouncedObjects.RemoveAt(self.bouncedObjects.Count - 1);
        w.MatchRelaxed(
            x => x.MatchLdcI4(1),
            x => x.MatchSub(),
            x => x.MatchCallvirt(out _) && w.SetCurrentTo(x),
            x => x.MatchLdarg(1),
            x => x.MatchLdarg(2)
        ).ThrowIfFailure();
        w.InsertAfterCurrent(w.Create(OpCodes.Ldarg_2));
        w.MarkLabelToCurrent(skipDoublingProc);
        w.InsertAfterCurrent(
            w.CreateCall(JustDealMoreDamage)
        );
    }
    private static void JustDealMoreDamage(LightningOrb lightningOrb)
    {
        if (!AllowErraticGadgetDamageBuff(lightningOrb.attacker))
        {
            return;
        }

        lightningOrb.damageValue *= GetErraticGadgetDamageMult(lightningOrb.attacker);
    }


    // charged perforator
    private static ReturnFlow SimpleLightningStrikeOrb_JustMoreDamage(SS2.Items.ErraticGadget self, ref On.RoR2.Orbs.SimpleLightningStrikeOrb.orig_OnArrival orig, ref RoR2.Orbs.SimpleLightningStrikeOrb simpleLightningStrikeOrb)
    {
        if (!AllowErraticGadgetDamageBuff(simpleLightningStrikeOrb.attacker))
        {
            orig(simpleLightningStrikeOrb);
            return ReturnFlow.SkipOriginal;
        }

        simpleLightningStrikeOrb.damageValue *= GetErraticGadgetDamageMult(simpleLightningStrikeOrb.attacker);

        orig(simpleLightningStrikeOrb);
        return ReturnFlow.SkipOriginal;
    }


    // royal capacitor
    private static ReturnFlow LightningStrikeOrb_JustMoreDamage(SS2.Items.ErraticGadget self, ref On.RoR2.Orbs.LightningStrikeOrb.orig_OnArrival orig, ref RoR2.Orbs.LightningStrikeOrb lightningStrikeOrb)
    {
        if (!AllowErraticGadgetDamageBuff(lightningStrikeOrb.attacker))
        {
            orig(lightningStrikeOrb);
            return ReturnFlow.SkipOriginal;
        }

        lightningStrikeOrb.damageValue *= GetErraticGadgetDamageMult(lightningStrikeOrb.attacker);

        orig(lightningStrikeOrb);
        return ReturnFlow.SkipOriginal;
    }


    private static bool AllowErraticGadgetDamageBuff(GameObject attacker)
    {
        if (attacker == null)
        {
            return false;
        }
        var attackerBody = attacker.GetComponent<CharacterBody>();
        if (attackerBody == null || attackerBody.inventory == null)
        {
            return false;
        }
        if (attackerBody.inventory.GetItemCountEffective(SS2Content.Items.ErraticGadget) < 1)
        {
            return false;
        }
        return true;
    }


    private static float GetErraticGadgetDamageMult(GameObject attacker)
    {
        // not using AllowErraticGadgetDamageBuff here bc i need to store the erratic gadget count
        // so without it it's one less GetItemCountEffective call
        if (attacker == null)
        {
            return 1;
        }
        var attackerBody = attacker.GetComponent<CharacterBody>();
        if (attackerBody == null || attackerBody.inventory == null)
        {
            return 1;
        }
        int currentErraticGadgetCount = attackerBody.inventory.GetItemCountEffective(SS2Content.Items.ErraticGadget);
        if (currentErraticGadgetCount < 1)
        {
            return 1;
        }

        if (ConfigOptions.ItemEdits.ErraticGadget.Value == ConfigOptions.ItemEdits.ErraticGadgetEditType.DamageMultAndOnHitProc)
        {
            return 2;
        }
        else if (ConfigOptions.ItemEdits.ErraticGadget.Value == ConfigOptions.ItemEdits.ErraticGadgetEditType.OnlyDamageMult)
        {
            return 3 + (1.5f * (currentErraticGadgetCount - 1));
        }

        return 1;
    }


    private static ReturnFlow ErraticGadget_OnDamageDealtServer(SS2.Items.ErraticGadget.Behavior self, ref DamageReport report)
    {
        Log.Warning("no ErraticGadget_OnDamageDealtServer");
        return ReturnFlow.SkipOriginal;
    }
}