using BepInEx;
using HarmonyLib;
using MonoDetour;
using RoR2;

namespace LordsStarstormEdits
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(SS2.SS2Main.GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(LordsItemEdits.Plugin.PluginGUID, BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static PluginInfo PluginInfo { get; private set; }
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "LordVGames";
        public const string PluginName = "LordsStarstormEdits";
        public const string PluginVersion = "1.0.0";
        public void Awake()
        {
            PluginInfo = Info;
            Log.Init(Logger);
            ConfigOptions.BindConfigOptions(Config);
            MyAssets.Init();
            MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly);
            ItemCatalog.availability.CallWhenAvailable(SS2Edits.Items.RemoveBrokenItemAchievements.YouShallWorkAgain);
        }
    }
}
