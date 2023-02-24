using ApplianceLib.Api;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.References;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraysPlus
{
    internal class ServingTrayStand : CustomAppliance
    {
        public override int BaseGameDataObjectID => ApplianceReferences.TrayStand;
        public override string Name => "Serving Tray Stand";
        public override string Description => "Be a real server with this.";
        public override string UniqueNameID => "ServingTrayStand";
        public override List<Appliance.Section> Sections
        {
            get
            {
                Appliance.Section section = new Appliance.Section();
                section.Title = "Tool - Serving Tray";
                section.Description = "Serve 4 plates at once!";
                return new(new Appliance.Section[] { section });
            }
        }

        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>(new IApplianceProperty[] {
        KitchenPropertiesUtils.GetCItemProvider(GDOUtils.GetCustomGameDataObject<ServingTray>().ID,1,1,false,false,false,false,false,true,false),
        new Kitchen.CItemHolder{}
        });
        
    }
}
