using BepInEx;
using MonoDetour;
using RoR2;
namespace StarstormSquared;


[BepInDependency(SS2.SS2Main.GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(R2API.LanguageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LordsItemEdits.Plugin.Id, BepInDependency.DependencyFlags.SoftDependency)]
[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    public static PluginInfo PluginInfo { get; private set; }
    public void Awake()
    {
        PluginInfo = Info;
        Log.Init(Logger);
        ConfigOptions.BindAllConfigOptions(Config);
        MyAssets.Init();
        MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly);
        ItemCatalog.availability.CallWhenAvailable(SS2Edits.Items.RemoveBrokenItemAchievements.YouShallWorkAgain);
        RoR2Application.onLoad += SS2Edits.Survivors.SurvivorIcons.ChangeSurvivorIcons;
    }
}
