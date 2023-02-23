using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Utils;
using KitchenMods;
using System.Reflection;
using UnityEngine;
using KitchenData;
using ApplianceLib.Api;
using TraysPlus;
using KitchenLib.References;
using Kitchen;
using KitchenLib.Customs;
using System.Collections.Generic;

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
        public const string MOD_VERSION = "0.1.0";
        public const string MOD_AUTHOR = "Dusk_Xua";
        public const string MOD_GAMEVERSION = ">=1.1.3";
        // Game version this mod is designed for in semver
        // e.g. ">=1.1.3" current and all future
        // e.g. ">=1.1.3 <=1.2.3" for all from/until

        public static Dictionary<string, int> references => new Dictionary<string, int>();


        public int ServingTrayStandID;
        public int ServingTrayID;
        public int DishTubStandID;
        public int DishTubID;

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


        }

        private void AddGameData()
        {
            LogInfo("Attempting to register game data...");

            ServingTray servingTray = AddGameDataObject<ServingTray>();
            DishTub dishTub = AddGameDataObject<DishTub>();


            ServingTrayStand servingTrayStand = AddGameDataObject<ServingTrayStand>();
            DishTubStand dishTubStand = AddGameDataObject<DishTubStand>();

            ServingTrayID = servingTray.ID;
            ServingTrayStandID = servingTrayStand.ID;
            DishTubID = dishTub.ID;
            DishTubStandID = dishTubStand.ID;


            //servingTrayStand.Properties.Add(KitchenPropertiesUtils.GetCItemProvider(ServingTrayID, 1, 1, false, false, true, false, false, true, false));
            //dishTubStand.Properties.Add(KitchenPropertiesUtils.GetCItemProvider(DishTubID, 1, 1, false, false, true, false, false, true, false));

            LogInfo("Done loading game data.");
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnPostActivate(KitchenMods.Mod mod)
        {
            // TODO: Uncomment the following if you have an asset bundle.
            // TODO: Also, make sure to set EnableAssetBundleDeploy to 'true' in your ModName.csproj

            // LogInfo("Attempting to load asset bundle...");
            // Bundle = mod.GetPacks<AssetBundleModPack>().SelectMany(e => e.AssetBundles).First();
            // LogInfo("Done loading asset bundle.");

            // Register custom GDOs
            AddGameData();


            // Perform actions when game data is built
            Events.BuildGameDataEvent += delegate (object s, BuildGameDataEventArgs args)
            {
                string menuItems = "Menu Items \n";

                foreach(Dish dish in GameData.Main.Get<Dish>())
                {
                    foreach(Dish.MenuItem item in dish.UnlocksMenuItems)
                    {
                        menuItems += item.Item.name + "\n";
                        RestrictedItemTransfers.AllowItem("ServingTray", item.Item);
                    }
                }
                //LogInfo(menuItems);


                Item[] plates = {
                    GameData.Main.Get<Item>(ItemReferences.Plate),
                    GameData.Main.Get<Item>(ItemReferences.PlateDirty),
                    GameData.Main.Get<Item>(ItemReferences.PlateDirtySoaked),
                    GameData.Main.Get<Item>(ItemReferences.PlateDirtywithBone),
                    GameData.Main.Get<Item>(ItemReferences.PlateDirtywithfood)
                };

                RestrictedItemTransfers.AllowItem("ServingTrayStand", GDOUtils.GetCustomGameDataObject<ServingTray>().ID);

                foreach (Item plate in plates)
                {
                    RestrictedItemTransfers.AllowItem("DishTub", plate);
                }


                Appliance trayStand = GameData.Main.Get<Appliance>(ApplianceReferences.TrayStand);
                Appliance servingTrayStand = GDOUtils.GetCastedGDO<Appliance, ServingTrayStand>();
                Appliance dishTubStand = GDOUtils.GetCastedGDO<Appliance, DishTubStand>();

                trayStand.Upgrades.Add(servingTrayStand);
                trayStand.Upgrades.Add(dishTubStand);

                dishTubStand.Upgrades.Add(servingTrayStand);
                servingTrayStand.Upgrades.Add(dishTubStand);
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
