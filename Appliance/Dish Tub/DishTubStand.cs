using ApplianceLib.Api.Prefab;
using ApplianceLib.Customs.GDO;
using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using KitchenTraysPlus;
using System.Collections.Generic;
using UnityEngine;

namespace TraysPlus
{
    internal class DishTubStand : ModAppliance
    {

        public override string UniqueNameID => "DishTubStand";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Dish_Tub_Counter");
        public override RarityTier RarityTier => RarityTier.Rare;
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override List<Appliance.ApplianceProcesses> Processes => ((Appliance)GDOUtils.GetExistingGDO(KitchenLib.References.ApplianceReferences.Countertop)).Processes;
        public override ShoppingTags ShoppingTags => ShoppingTags.FrontOfHouse | ShoppingTags.Misc;
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
            var materials = new Material[] { MaterialUtils.GetExistingMaterial("Metal Very Dark") };
            MaterialUtils.ApplyMaterial(prefab, "HoldPoint/Dish_Tub/Cube", materials);
            
            PrefabBuilder.AttachCounter(prefab, CounterType.DoubleDoors);
            
            var holdTransform = GameObjectUtils.GetChildObject(prefab,"HoldPoint").transform;
            var holdPoint = prefab.AddComponent<HoldPointContainer>();
            holdPoint.HoldPoint = holdTransform;

            var sourceView = prefab.AddComponent<LimitedItemSourceView>();
            sourceView.HeldItemPosition = holdTransform;

            ReflectionUtils.GetField<LimitedItemSourceView>("Items").SetValue(sourceView, new List<GameObject>()
            {
                GameObjectUtils.GetChildObject(prefab, "HoldPoint/Dish_Tub")
            });
        }

    }
}
