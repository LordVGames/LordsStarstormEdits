using BepInEx;
using MonoDetour;
using RoR2;
[assembly: HG.Reflection.SearchableAttribute.OptIn]
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
        LoadedAssets.LoadAssets();
        MonoDetourManager.InvokeHookInitializers(typeof(Plugin).Assembly);
        RoR2Application.onLoad += Changes.Survivors.SurvivorIcons.ChangeSurvivorIcons;
        RoR2Application.onLoad += Changes.Survivors.DUT.GiveSkillIcons.GiveDUTSkillIcons;
        RoR2Application.onLoad += Changes.Survivors.DUT.MakeDriftBuffNotHidden.EditDriftBuff;
    }
}