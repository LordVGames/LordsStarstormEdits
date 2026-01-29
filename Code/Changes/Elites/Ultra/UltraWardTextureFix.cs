using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using SS2;
using UnityEngine;
using Mono.Cecil.Cil;
using MonoDetour;
using MonoDetour.Cil;
using MonoDetour.DetourTypes;
using MonoDetour.HookGen;
using MonoMod.Cil;
using SS2.Items;
namespace StarstormSquared.Changes.Elites.Ultra;


[MonoDetourTargets(typeof(AffixUltra), GenerateControlFlowVariants = true)]
internal static class UltraWardTextureFix
{
    private static Vector2 _cloud1TexScale = new(0.1f, 0.08f);
    private static Vector2 _mainTexScale = new(0.7f, 0.11f);
    private static Color32 _tintColor = new(88, 255, 0, 200);

    [MonoDetourHookInitialize]
    internal static void Setup()
    {
        if (!SS2Config.enableBeta.value)
        {
            return;
        }

        Mdh.SS2.Items.AffixUltra.Initialize.ILHook(EditUltraWard);
    }


    private static void EditUltraWard(ILManipulationInfo info)
    {
        ILWeaver w = new(info);

        w.MatchRelaxed(
            x => x.MatchStsfld<AffixUltra>("_wardPrefab") && w.SetCurrentTo(x)
        ).ThrowIfFailure()
        .InsertBeforeCurrent(
            w.CreateDelegateCall((GameObject ward) =>
            {
                Transform indicator = ward.transform.GetChild(0).GetChild(0);
                if (!indicator.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
                {
                    return ward;
                }
                Material meshMaterial = meshRenderer.GetSharedMaterial();

                meshMaterial.SetTextureScale("_Cloud1Tex", _cloud1TexScale);
                meshMaterial.SetTextureScale("_MainTex", _mainTexScale);
                meshMaterial.SetColor("_TintColor", _tintColor);

                return ward;
            })
        );
    }
}