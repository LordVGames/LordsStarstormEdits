using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;

namespace LordsStarstormEdits.SS2Edits.Items
{
    internal static class RemoveBrokenItemAchievements
    {
        internal static void YouShallWorkAgain()
        {
            if (SS2Content.Items.X4.unlockableDef != null)
            {
                SS2Content.Items.X4.unlockableDef = null;
                SS2Content.Items.Insecticide.unlockableDef = null;
            }
        }
    }
}