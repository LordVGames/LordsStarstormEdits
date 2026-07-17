using MonoDetour;
using MonoDetour.HookGen;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;
namespace StarstormSquared.Changes.Survivors.DUT;


internal static class MakeDriftBuffNotHidden
{
    // TODO shit doesnt work
    //[SystemInitializer(dependencies: typeof(BuffCatalog))]
    internal static void EditDriftBuff()
    {
        if (!SS2Config.enableBeta.value || SS2Content.Buffs.bdDUTDrift == null)
        {
            return;
        }


        SS2Content.Buffs.bdDUTDrift.iconSprite = LoadedAssets.ToolbotDashIcon;
        SS2Content.Buffs.bdDUTDrift.isHidden = false;
    }
}