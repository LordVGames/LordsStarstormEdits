using System;
using SS2;
using MonoDetour;
using MonoDetour.HookGen;
using MonoDetour.Cil;
namespace StarstormSquared.SS2Edits.EtherealStuff;


[MonoDetourTargets(typeof(EtherealBehavior))]
internal static class ConfigSaplingReplacingNewtAltars
{
    [MonoDetourHookInitialize]
    private static void Setup()
    {
        Mdh.SS2.EtherealBehavior.SpawnShrine.Prefix(ChangeBasedOnConfig);
    }

    private static void ChangeBasedOnConfig(EtherealBehavior self)
    {
        EtherealBehavior.alwaysReplaceNewts = ConfigOptions.Ethereal.ChangeReplaceNewtWithEthereal.Value;
    }
}