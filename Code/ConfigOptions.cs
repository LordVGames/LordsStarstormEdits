using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using MiscFixes.Modules;
namespace StarstormSquared;


public static class ConfigOptions
{
    public static class Chirr
    {
        public static ConfigEntry<bool> ChirrMinionsNoHealingItems;
        public static ConfigEntry<bool> ChirrNoTamingTerminals;
        public static ConfigEntry<bool> ChirrTameLingerFix;

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
        }
    }

    public static class Knight
    {
        public static ConfigEntry<bool> AddKnightPassive;

        internal static void BindConfigOptions(ConfigFile config)
        {
            AddKnightPassive = config.BindOption(
                "Knight",
                "Add his unused passive",
                "Knight has a (currently) unused passive buff ward for allies that gives some attack speed and movement speed. This implements that.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
        }
    }

    public static class Events
    {
        public static ConfigEntry<bool> NewStormEventText;
        public static ConfigEntry<bool> NewSuperEliteSpawnEventText;

        internal static void BindConfigOptions(ConfigFile config)
        {
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
        }
    }

    public static class Elites
    {
        public static class Empyrean
        {
            private const string _sectionName = "Empyrean Elites";
            public static ConfigEntry<bool> RemoveEmpyreanShardDrop;
            public static ConfigEntry<bool> AllowEmpyreansInJudgement;
            public static ConfigEntry<int> CustomEmpyreanLevelUpInterval;

            internal static void BindConfigOptions(ConfigFile config)
            {
                RemoveEmpyreanShardDrop = config.BindOption(
                    _sectionName,
                    "Remove shard drop",
                    "Prevents empyrean elites from dropping a random shard on death.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
                AllowEmpyreansInJudgement = config.BindOption(
                    _sectionName,
                    "Allow spawning during EnemiesReturns Judgement",
                    "Empyreans can replace the normal aeonian spawns during the 1st Arraign phase, enable this if you still want that.",
                    false,
                    Extensions.ConfigFlags.RestartRequired
                );
                CustomEmpyreanLevelUpInterval = config.BindOptionSlider(
                    _sectionName,
                    "Custom Empyrean Level Up Interval",
                    "Empyreans level up and gain doubled stats + a lot of HP every number of stages after the first stage they can appear on, that being stage 9. Change this number to change how many stages it takes for empyreans to level up.",
                    5
                );
            }
        }


        public static class Ultra
        {
            public static ConfigEntry<bool> TweakUltraWardBuff;
            public static ConfigEntry<bool> AddHealingToUltraWardBuff;


            internal static void BindConfigOptions(ConfigFile config)
            {
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
            }
        }

        public static class Toxic
        {
            private const string _sectionName = "Toxic Elites";
            public static ConfigEntry<bool> EnableToxicElite;

            internal static void BindConfigOptions(ConfigFile config)
            {
                EnableToxicElite = config.BindOption(
                    _sectionName,
                    "Enable elite",
                    "Toxic elites can't be disabled for some reason, so use this if you want to remove them.",
                    true,
                    Extensions.ConfigFlags.RestartRequired
                );
            }
        }


        internal static void BindAllConfigOptions(ConfigFile config)
        {
            Empyrean.BindConfigOptions(config);
            Ultra.BindConfigOptions(config);
            Toxic.BindConfigOptions(config);
        }
    }

    public static class Ethereal
    {
        public static ConfigEntry<bool> ImplementZanzanPortalAppearText;
        public static ConfigEntry<bool> AddCraftingChefToZanzanStage;
        public static ConfigEntry<bool> ChangeReplaceNewtWithEthereal;

        internal static void BindConfigOptions(ConfigFile config)
        {
            ImplementZanzanPortalAppearText = config.BindOption(
                "Ethereal Related",
                "Implement unused Zanzan portal appearance text",
                "There's already added non-placeholder text for when Zanzan's portal appears, this will implement it.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
            AddCraftingChefToZanzanStage = config.BindOption(
                "Ethereal Related",
                "Add a crafting chef to the strangers hideout",
                "It is put on top of the roof that Zanzan the faded sits under.",
                true
            );
            ChangeReplaceNewtWithEthereal = config.BindOption(
                "Ethereal Related",
                "Make ethereal sapling spawn in stage-specific spots instead of newt altar spots.",
                "In stages without a pre-determined spot they will still spawn in a newt atltar spot.",
                false
            );
        }
    }

    public static class Shards
    {
        public static ConfigEntry<bool> RestoreGoldShardDrop;
        public static ConfigEntry<bool> RestoreVoidShardDrop;
        //public static ConfigEntry<bool> RestoreStormShardDrops;
        public static ConfigEntry<bool> RestoreSuperEliteShardDrops;

        internal static void BindConfigOptions(ConfigFile config)
        {
            string otherShardDropCategoryName = "Restore Shard Drops";

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
        }
    }

    public static class ItemEdits
    {
        public static ConfigEntry<bool> ArmedBackpack;

        public enum ErraticGadgetEditType
        {
            None = 0,
            DamageMultAndOnHitProc,
            OnlyDamageMult
        }
        public static ConfigEntry<ErraticGadgetEditType> ErraticGadget;

        public static ConfigEntry<bool> PortableReactor;

        internal static void BindConfigOptions(ConfigFile config)
        {
            ArmedBackpack = config.BindOption(
                "Other Item Edits",
                "Armed Backpack",
                "Replaces the missile projectile with a missile orb, similar to the ones plasma shrimp uses.",
                true
            );
            ErraticGadget = config.BindOption(
                "Other Item Edits",
                "Erratic Gadget",
                "2 Different edits:\n\nDamageMultAndOnHitProc: Doubled lightning damage and chance to do chain lightning on hit. Stacks increase chance and targets hit.\n\nOnlyDamageMult: 3x lightning damage, stacks add to the damage multiplier.\n\nAnd of course a None option for if you don't want either.",
                ErraticGadgetEditType.DamageMultAndOnHitProc,
                Extensions.ConfigFlags.RestartRequired
            );
            PortableReactor = config.BindOption(
                "Other Item Edits",
                "Portable Reactor",
                "Makes portable reactor give 100 armor instead of invulnerability while active.",
                true,
                Extensions.ConfigFlags.RestartRequired
            );
        }
    }

    internal static void BindAllConfigOptions(ConfigFile config)
    {
        Chirr.BindConfigOptions(config);
        Knight.BindConfigOptions(config);
        Events.BindConfigOptions(config);
        Elites.BindAllConfigOptions(config);
        Ethereal.BindConfigOptions(config);
        Shards.BindConfigOptions(config);
        ItemEdits.BindConfigOptions(config);
    }
}
