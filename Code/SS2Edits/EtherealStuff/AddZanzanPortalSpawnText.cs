using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using RoR2;
using SS2;
using System;
using System.Collections.Generic;
using System.Text;

namespace LordsStarstormEdits.SS2Edits.EtherealStuff
{
    // the token for this exists in SS2 already, it's just not used for some reason
    [MonoDetourTargets(typeof(SS2.TeleporterUpgradeController))]
    internal static class AddZanzanPortalSpawnText
    {
        [MonoDetourHookInitialize]
        internal static void Setup()
        {
            if (!ConfigOptions.ImplementZanzanPortalAppearText.Value || !SS2Config.enableBeta.value)
            {
                return;
            }

            MonoDetourHooks.SS2.TeleporterUpgradeController.OnTeleporterChargedGlobal.ILHook(ImplementExistingText);
        }

        private static void ImplementExistingText(ILManipulationInfo info)
        {
            ILWeaver w = new(info);

            // r.i.p oortal
            w.MatchRelaxed(
                x => x.MatchLdstr("hehe oortal") && w.SetCurrentTo(x)
            ).ThrowIfFailure();
            w.Current.Operand = "SS2_PORTAL_VOIDSHOP_APPEAR";
        }
    }
}