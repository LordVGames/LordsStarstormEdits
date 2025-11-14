using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using MiscFixes.Modules;

namespace LordsStarstormEdits
{
    public static class ConfigOptions
    {
        public static ConfigEntry<bool> ChirrMinionsNoHealingItems;
        public static ConfigEntry<bool> ChirrNoTamingTerminals;
        public static ConfigEntry<bool> ChirrTameLingerFix;

        public static ConfigEntry<bool> AddKnightPassive;

        public static ConfigEntry<bool> NewStormEventText;
        public static ConfigEntry<bool> NewSuperEliteSpawnEventText;

        public static ConfigEntry<bool> RemoveEmpyreanShardDrop;
        public static ConfigEntry<bool> AllowEmpyreansInJudgement;
        public static ConfigEntry<bool> EnableToxicElite;

        public static ConfigEntry<bool> ImplementZanzanPortalAppearText;
        public static ConfigEntry<bool> TweakUltraWardBuff;
        public static ConfigEntry<bool> AddHealingToUltraWardBuff;

        public static ConfigEntry<bool> RestoreGoldShardDrop;
        public static ConfigEntry<bool> RestoreVoidShardDrop;
        //public static ConfigEntry<bool> RestoreStormShardDrops;
        public static ConfigEntry<bool> RestoreSuperEliteShardDrops;

        internal static void BindConfigOptions(ConfigFile config)
        {
            ChirrMinionsNoHealingItems = config.BindOption(
                "Chirr",
                "Make tamed enemies not get Chirrs healing items.",
                "This is so her gameplay of keeping her tamed enemy healthy isn't removed once a few good healing items are required.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            ChirrNoTamingTerminals = config.BindOption(
                "Chirr",
                "Make enemies from Relic of Termination untamable",
                "Termination eneimes are insanely strong when tamed by Chirr, this makes them untamable so they're not an almost free win",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            ChirrTameLingerFix = config.BindOption(
                "Chirr",
                "Fix enemies being tamable after debuff expiration",
                "Enemies can be tamed even after the taming debuff expires, this fixes that.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );


            AddKnightPassive = config.BindOption(
                "Knight",
                "Add his unused passive",
                "Knight has a (currently) unused passive buff ward for allies that gives some attack speed and movement speed. This implements that.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );


            NewStormEventText = config.BindOption(
                "Storms",
                "New storm text",
                "Replaces the silly placeholder storm event text with new text.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            NewSuperEliteSpawnEventText = config.BindOption(
                "Elite Events",
                "New super elite spawn text",
                "Replaces the silly placeholder super elite spawn event text with new text.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );


            RemoveEmpyreanShardDrop = config.BindOption(
                "Empyreans",
                "Remove shard drop",
                "Prevents empyrean elites from dropping a random shard on death.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            AllowEmpyreansInJudgement = config.BindOption(
                "Empyreans",
                "Allow spawning during EnemiesReturns Judgement",
                "Empyreans can replace the normal aeonian spawns during the 1st Arraign phase, enable this if you still want that.",
                false,
                Extensions.ConfigFlags.RestartRequired
            );
            EnableToxicElite = config.BindOption(
                "Toxic Elites",
                "Enable elite",
                "Toxic elites can't be disabled for some reason, so use this if you want to remove them.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );


            ImplementZanzanPortalAppearText = config.BindOption(
                "Ethereal Related",
                "Implement unused Zanzan portal appearance text",
                "There's already added non-placeholder text for when Zanzan's portal appears, this will implement it.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            TweakUltraWardBuff = config.BindOption(
                "Ethereal Related",
                "Tweak the passive buff from ultra elites",
                "Makes the passive buff ultra elites give off also apply to the ultra elite itself, along with the passive buff giving a tiny amount of % hp regen to everyone but ultra elites. This basically makes it like more of a super duper mending elite.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            AddHealingToUltraWardBuff = config.BindOption(
                "Ethereal Related",
                "Add slight healing to passive buff from ultra elites",
                "The healing added is currently jank and heals way more than it should, if you still want it then enable this setting.",
                false,
                Extensions.ConfigFlags.RestartRequired
            );


            string otherShardDropCategoryName = "Restored Shard Drop Sources";
            RestoreGoldShardDrop = config.BindOption(
                otherShardDropCategoryName,
                "Restore gold shard drop",
                "Gold shards drop from completed halcyon shrines.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            RestoreVoidShardDrop = config.BindOption(
                otherShardDropCategoryName,
                "Restore void shard drop",
                "Void shards drop from completed void seeds.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            /*RestoreStormShardDrops = config.BindOption(
                otherShardDropCategoryName,
                "Restore storm shard drops",
                "Storm shards drop when the boss from a stormborn teleporter is beaten.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );*/
            RestoreSuperEliteShardDrops = config.BindOption(
                otherShardDropCategoryName,
                "Restore shard drops from super elites",
                "Super elites drop shards on death based on the type of super elite they were.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );


            config.WipeConfig();
        }
    }
}