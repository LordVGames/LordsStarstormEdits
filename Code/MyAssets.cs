using System.IO;
using UnityEngine;

namespace LordsStarstormEdits
{
    internal static class MyAssets
    {
        internal static void Init()
        {
            ShardIcons.Init();
            SurvivorIcons.Init();
        }

        internal static class ShardIcons
        {
            public static AssetBundle AssetBundle;
            public const string BundleName = "lse_shard_icons";

            public static string AssetBundlePath
            {
                get
                {
                    return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
                }
            }

            internal static void Init()
            {
                AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
            }
        }

        internal static class SurvivorIcons
        {
            public static AssetBundle AssetBundle;
            public const string BundleName = "lse_survivor_icons";

            public static string AssetBundlePath
            {
                get
                {
                    return Path.Combine(Path.GetDirectoryName(Plugin.PluginInfo.Location), BundleName);
                }
            }

            internal static void Init()
            {
                AssetBundle = AssetBundle.LoadFromFile(AssetBundlePath);
            }
        }
    }
}
