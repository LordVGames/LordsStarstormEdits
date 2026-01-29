using System;
using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using RoR2.ContentManagement;
using SS2;
using UnityEngine.AddressableAssets;
namespace StarstormSquared.Changes.Items.Shards;


internal static class FixWithSubstandardDuplicator
{
    // this also fixes substandard duplicator shenanigans so this is good
    [SystemInitializer(dependencies: typeof(ItemCatalog))]
    private static void MakeCurioWorldUnique()
    {
        ItemTierDef curioItemTier = SS2Assets.FindAsset<ItemTierDef>("Curio");
        ItemIndex itemIndex = ItemIndex.Count;
        for (ItemIndex itemCount = (ItemIndex)ItemCatalog.itemCount; itemIndex < itemCount; itemIndex++)
        {
            ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
            if (itemDef._itemTierDef == curioItemTier)
            {
                itemDef.tags = [.. itemDef.tags, ItemTag.WorldUnique];
            }
        }
    }
}