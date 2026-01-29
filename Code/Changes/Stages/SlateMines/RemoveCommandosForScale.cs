using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace StarstormSquared.Changes.Stages.SlateMines;


internal static class RemoveCommandosForScale
{
    internal static void DeleteCommandos(Transform gameplayObjectsHolder)
    {
        Transform group = gameplayObjectsHolder.Find("slatemines4");
        if (group == null)
        {
            return;
        }


        for (int i = 0; i < group.childCount; i++)
        {
            Transform child = group.GetChild(i);
            if (child == null || !child.name.Contains("CommandoMesh"))
            {
                continue;
            }
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }
}