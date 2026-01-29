using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Items;


// this is called in Plugin.cs via ItemCatalog
internal static class RemoveBrokenItemAchievements
{
    internal static void YouShallWorkAgain()
    {
        if (SS2Content.Items.X4.unlockableDef != null)
        {
            SS2Content.Items.X4.unlockableDef = null;
            SS2Content.Items.Insecticide.unlockableDef = null;
            SS2Content.Items.FieldAccelerator.unlockableDef = null;
        }
    }
}