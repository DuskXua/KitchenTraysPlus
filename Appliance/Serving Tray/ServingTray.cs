using Kitchen;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenTraysPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraysPlus
{
    internal class ServingTray : CustomItem
    {
        public override int BaseGameDataObjectID => ItemReferences.Tray;
        public override string UniqueNameID => "ServingTray";
        public override List<IItemProperty> Properties => new(new IItemProperty[] {
            new CToolStorage{Capacity = 4},
            new CEquippableTool{CanHoldItems = false},
            new CRestrictedToolStorage{ItemKey = "ServingTray"}
        });
    }
}
