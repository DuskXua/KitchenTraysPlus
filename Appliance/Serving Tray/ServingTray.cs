using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using KitchenTraysPlus;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TraysPlus
{
    internal class ServingTray : CustomItem
    {
        public override int BaseGameDataObjectID => ItemReferences.Tray;
        public override string UniqueNameID => "ServingTray";
        public override GameObject Prefab => Mod.Bundle.LoadAsset<GameObject>("Serving_Tray");
        public override List<IItemProperty> Properties => new(new IItemProperty[] {
            new CToolStorage{Capacity = 4},
            new CEquippableTool{CanHoldItems = false},
            new CRestrictedToolStorage{ItemKey = "ServingTray"},
            new CSlowPlayer{Factor = .8f}
        });
        public override void OnRegister(GameDataObject gameDataObject)
        {
            base.OnRegister(gameDataObject);

            var materials = new Material[1];

            materials[0] = MaterialUtils.GetExistingMaterial("Danger Hob");
            MaterialUtils.ApplyMaterial(Prefab, "Cylinder", materials);

            FieldInfo storage = ReflectionUtils.GetField<ItemVariableStorageView>("Storage");
            List<GameObject> storages = new()
            {
                GameObjectUtils.GetChildObject(Prefab, "Storage 1"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 2"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 3"),
                GameObjectUtils.GetChildObject(Prefab, "Storage 4")
            };

            ItemVariableStorageView ivsv = Prefab.AddComponent<ItemVariableStorageView>();
            storage.SetValue(ivsv, storages);
        }
    }
}
