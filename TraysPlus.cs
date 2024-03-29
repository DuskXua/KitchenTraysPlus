﻿using ApplianceLib.Api;
using Kitchen;
using KitchenData;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenMods;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Namespace should have "Kitchen" in the beginning
namespace KitchenTraysPlus
{

    public class Mod : BaseMod, IModSystem
    {
        // GUID must be unique and is recommended to be in reverse domain name notation
        // Mod Name is displayed to the player and listed in the mods menu
        // Mod Version must follow semver notation e.g. "1.2.3"
        public const string MOD_GUID = "Xua.PlateUp.TraysPlus";
        public const string MOD_NAME = "Trays Plus";
        public const string MOD_VERSION = "0.2.4";
        public const string MOD_AUTHOR = "Dusk_Xua";
        public const string MOD_GAMEVERSION = ">=1.1.3";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until


        // Boolean constant whose value depends on whether you built with DEBUG or RELEASE mode, useful for testing
#if DEBUG
        public const bool DEBUG_MODE = true;
#else
        public const bool DEBUG_MODE = false;
#endif

        public static AssetBundle Bundle;

        public Mod() : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        {
            LogWarning($"{MOD_GUID} v{MOD_VERSION} in use!");
            Appliance servingTrayStand = GDOUtils.GetCastedGDO<Appliance, ServingTrayStand>();
            Appliance dishTubStand = GDOUtils.GetCastedGDO<Appliance, DishTubStand>();
            Appliance trayStand = GDOUtils.GetExistingGDO(ApplianceReferences.TrayStand) as Appliance;

            dishTubStand.Upgrades.Add(servingTrayStand);
            servingTrayStand.Upgrades.Add(dishTubStand);

            trayStand.Upgrades.Add(servingTrayStand);
            trayStand.Upgrades.Add(dishTubStand);

        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            AddGameDataObject<ServingTray>();
            AddGameDataObject<DishTub>();

            AddGameDataObject<ServingTrayStand>();
            AddGameDataObject<DishTubStand>();

            RestrictedItemTransfers.AllowItem("DishTub", ItemReferences.Plate);
            RestrictedItemTransfers.AllowItem("DishTub", ItemReferences.PlateDirty);
            RestrictedItemTransfers.AllowItem("DishTub", ItemReferences.PlateDirtySoaked);
            RestrictedItemTransfers.AllowItem("DishTub", ItemReferences.PlateDirtywithBone);
            RestrictedItemTransfers.AllowItem("DishTub", ItemReferences.PlateDirtywithfood);

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        
        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // Load asset bundle
            LogInfo("Attempting to load asset bundle...");
            Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();

            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                if (args.firstBuild)
                {
                    LogInfo("Add Dishes");
                    // Add Dishes to Serving Tray Allow List
                    foreach (Dish dish in args.gamedata.Get<Dish>())
                    {
                        foreach(Dish.MenuItem item in dish.UnlocksMenuItems)
                        {
                            RestrictedItemTransfers.AllowItem("ServingTray", item.Item);
                            //LogInfo(item.Item.name);
                        }
                    }
                    RestrictedItemTransfers.AllowItem("ServingTray", ItemReferences.Plate);
                }
            };
        }
        #region Logging
        public static void LogInfo(string _log) { Debug.Log($"[{MOD_NAME}] " + _log); }
        public static void LogWarning(string _log) { Debug.LogWarning($"[{MOD_NAME}] " + _log); }
        public static void LogError(string _log) { Debug.LogError($"[{MOD_NAME}] " + _log); }
        public static void LogInfo(object _log) { LogInfo(_log.ToString()); }
        public static void LogWarning(object _log) { LogWarning(_log.ToString()); }
        public static void LogError(object _log) { LogError(_log.ToString()); }
        #endregion
    }
}
