using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenTraysPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TraysPlus
{
    internal class DishTub : CustomItem
    {
        public override int BaseGameDataObjectID => ItemReferences.Tray;
        public override string UniqueNameID => "DishTub";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Dish_Tub");
        public override List<IItemProperty> Properties => new(new IItemProperty[] {
            new CToolStorage{Capacity = 5},
            new CEquippableTool{CanHoldItems = false},
            new CRestrictedToolStorage{ItemKey = "DishTub"}
        });
        public override void OnRegister(GameDataObject gameDataObject)
        {
            base.OnRegister(gameDataObject);

            var materials = new Material[] { MaterialUtils.GetExistingMaterial("Metal Very Dark")};
            MaterialUtils.ApplyMaterial(Prefab, "Cube", materials);

            FieldInfo storage = ReflectionUtils.GetField<ItemVariableStorageView>("Storage");
            List<GameObject> storages = new()
            {
                Prefab.transform.Find("Storage 1").gameObject,
                Prefab.transform.Find("Storage 2").gameObject,
                Prefab.transform.Find("Storage 3").gameObject,
                Prefab.transform.Find("Storage 4").gameObject,
                Prefab.transform.Find("Storage 5").gameObject
            };

            ItemVariableStorageView ivsv = Prefab.AddComponent<ItemVariableStorageView>();
            storage.SetValue(ivsv, storages);
        }
    }
}
