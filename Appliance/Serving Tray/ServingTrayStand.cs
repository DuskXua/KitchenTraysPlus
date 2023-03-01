using ApplianceLib.Api;
using ApplianceLib.Customs.GDO;
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

namespace TraysPlus
{
    internal class ServingTrayStand : ModAppliance
    {
        public override int BaseGameDataObjectID => ApplianceReferences.TrayStand;
        public override string UniqueNameID => "ServingTrayStand";
        public override bool IsPurchasable => false;
        public override bool IsPurchasableAsUpgrade => true;
        public override List<Appliance.ApplianceProcesses> Processes => ((Appliance)GDOUtils.GetExistingGDO(KitchenLib.References.ApplianceReferences.Countertop)).Processes;

        public override List<(Locale, ApplianceInfo)> InfoList => new()
        {
            (Locale.English, LocalisationUtils.CreateApplianceInfo(
                "Serving Tray Stand",
                "Be a real server with this.",
                new List<Appliance.Section>() { new Appliance.Section() { Title = "Tool - Serving Tray", Description = "Serve 4 plates at once!" } },
                new()))
        };
        
        public override List<IApplianceProperty> Properties => new List<IApplianceProperty>(new IApplianceProperty[] {
        KitchenPropertiesUtils.GetCItemProvider(GDOUtils.GetCustomGameDataObject<ServingTray>().ID,1,1,false,false,true,false,false,true,false),
        new Kitchen.CItemHolder{}
        });

        protected override void SetupPrefab(GameObject prefab)
        {
            Material[] materials = new Material[] { MaterialUtils.GetExistingMaterial("Wood 4 - Painted") };
            MaterialUtils.ApplyMaterial(prefab, "Counter/Block/Counter2/Counter", materials);
            MaterialUtils.ApplyMaterial(prefab, "Counter/Block/Counter2/Counter Doors", materials);

            materials = new Material[] { MaterialUtils.GetExistingMaterial("Wood - Default") };
            MaterialUtils.ApplyMaterial(prefab, "Counter/Block/Counter2/Counter Surface", materials);
            MaterialUtils.ApplyMaterial(prefab, "Counter/Block/Counter2/Counter Top", materials);

            materials = new Material[] { MaterialUtils.GetExistingMaterial("Knob") };
            MaterialUtils.ApplyMaterial(prefab, "Counter/Block/Counter2/Handles", materials);



        }
    }
}
