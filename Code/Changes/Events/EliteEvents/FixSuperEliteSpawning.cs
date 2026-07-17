using MonoDetour;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Events.EliteEvents;


[MonoDetourTargets(typeof(EliteEventMissionController), GenerateControlFlowVariants = true)]
internal static class FixSuperEliteSpawning
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EliteEventMissionController.OnSpawnedServer.ControlFlowPrefix(DontAffectSuperElite);
    }


    private static ReturnFlow DontAffectSuperElite(EliteEventMissionController self, ref GameObject masterObject)
    {
        if (
            masterObject == null
            || !masterObject.TryGetComponent<Inventory>(out var inventory)
            || IsEquipmentSuperElite(inventory)
        )
        {
            return ReturnFlow.SkipOriginal;
        }
        return ReturnFlow.None;
    }


    private static bool IsEquipmentSuperElite(Inventory inventory)
    {
        EquipmentIndex equipmentIndex = inventory.GetEquipmentIndex();
        // can't do a switch here sadly
        return equipmentIndex == LoadedAssets.superFireEquipmentDef.equipmentIndex
            || equipmentIndex == LoadedAssets.superIceEquipmentDef.equipmentIndex
            || equipmentIndex == LoadedAssets.superEarthEquipmentDef.equipmentIndex
            || equipmentIndex == LoadedAssets.superLightningEquipmentDef.equipmentIndex;
    }
}