using ApplianceLib.Api;
using ApplianceLib.Util;
using ApplianceLib.Customs.GDO;
using ApplianceLib.Api.Prefab;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kitchen;
using KitchenTraysPlus;

namespace TraysPlus
{
    internal class DishTubStand : ModAppliance
    {

        //public override int BaseGameDataObjectID => ApplianceReferences.Countertop;
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Dish_Tub_Counter");
        public override string UniqueNameID => "DishTubStand";
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override List<Appliance.ApplianceProcesses> Processes => ((Appliance)GDOUtils.GetExistingGDO(KitchenLib.References.ApplianceReferences.Countertop)).Processes;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo(
                "Dish Tub",
                "Be a real Bus Boy with this.", 
                new List<Appliance.Section>() { new Appliance.Section() { Title = "Tool - Dish Tub", Description = "Clear 5 plates at once!" } }, 
                new()))
        };
        
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>(new IApplianceProperty[] {
        KitchenPropertiesUtils.GetCItemProvider(GDOUtils.GetCustomGameDataObject<DishTub>().ID,1,1,false,false,true,false,false,true,false),
        new Kitchen.CItemHolder{}
        });



        protected override void SetupPrefab(GameObject prefab)
        {
            Mod.LogInfo("Get Material");
            Material[] materials = new Material[] { MaterialUtils.GetExistingMaterial("Metal Very Dark") };
            Mod.LogInfo("Apply Material");
            MaterialUtils.ApplyMaterial(prefab, "HoldPoint/Dish_Tub/Cube", materials);


            Mod.LogInfo("Attach Counter");
            PrefabBuilder.AttachCounter(prefab, CounterType.Drawers);
            //PrefabBuilder.AttachPrefabAsChild();

            Mod.LogInfo("Connect Tub to HoldPoint");
            var holdTransform = GameObjectUtils.GetChildObject(prefab,"HoldPoint").transform;
            var holdPoint = prefab.AddComponent<HoldPointContainer>();
            holdPoint.HoldPoint = holdTransform;

            Mod.LogInfo("Connect to SorceView");
            var sourceView = prefab.AddComponent<LimitedItemSourceView>();
            sourceView.HeldItemPosition = holdTransform;

            Mod.LogInfo("Reflection");
            ReflectionUtils.GetField<LimitedItemSourceView>("Items").SetValue(sourceView, new List<GameObject>()
            {
                GameObjectUtils.GetChildObject(prefab, "HoldPoint/Dish_Tub")
            });

            Mod.LogInfo("Done!");
        }

    }
}
