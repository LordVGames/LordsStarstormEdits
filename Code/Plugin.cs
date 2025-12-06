using BepInEx;
using MonoDetour;
namespace LordsStarstormEdits;


[BepInAutoPlugin]
[BepInDependency(SS2.SS2Main.GUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(R2API.LanguageAPI.PluginGUID, BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency(LordsItemEdits.Plugin.PluginGUID, BepInDependency.DependencyFlags.SoftDependency)]
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
    }
}