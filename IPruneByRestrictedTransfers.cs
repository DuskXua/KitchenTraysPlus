using ApplianceLib.Api;
using Kitchen;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TraysPlus
{
    
    [UpdateInGroup(typeof(ItemTransferEarlyPrune))]
    internal class IPruneByRestrictedTransfers : GenericSystemBase, IModSystem
    {
        private EntityQuery TransferProposals;
        internal static EntityManager AttachedEntityManager;
        private static IPruneByRestrictedTransfers Instance;

        protected override void Initialise()
        {
            TransferProposals = GetEntityQuery(
                typeof(CItemTransferProposal)
            );
            AttachedEntityManager = EntityManager;
            Instance = this;
            
        }

        protected override void OnUpdate()
        {
            using var entities = TransferProposals.ToEntityArray(Allocator.Temp);
            using var itemTransferProposals = TransferProposals.ToComponentDataArray<CItemTransferProposal>(Allocator.Temp);


            for (int i = 0; i < entities.Length; ++i)
            {
                var entity = entities[i];
                var proposal = itemTransferProposals[i];

                //KitchenTraysPlus.Mod.LogInfo("Item: " + GDOUtils.GetExistingGDO(proposal.ItemData.ID).name + " - " + proposal.Source.ToString() + " => " + proposal.Destination.ToString());

                if (proposal.Status == ItemTransferStatus.Pruned)
                {
                    continue;
                }
                


                if (Require(proposal.Source, out CRestrictedToolStorage source))
                {
                    //KitchenTraysPlus.Mod.LogInfo("Source: Tried " + GDOUtils.GetExistingGDO(proposal.ItemData.ID).name + " => " + source.ItemKey);
                    
                    string itemKey = source.ItemKey.ToString();
                    if (!RestrictedItemTransfers.IsAllowed(itemKey, proposal.ItemType))
                    {
                        proposal.Status = ItemTransferStatus.Pruned;
                    }
                }
                else if (Require(proposal.Destination, out CToolUser toolUser))
                {
                    

                    //KitchenTraysPlus.Mod.LogInfo("Destination: Tried " + proposal.ItemType + " <= Player");

                    if(Require(toolUser.CurrentTool, out CRestrictedToolStorage destination))
                    {
                        string itemKey = destination.ItemKey.ToString();
                        if (!RestrictedItemTransfers.IsAllowed(itemKey, proposal.ItemType))
                        {
                            proposal.Status = ItemTransferStatus.Pruned;
                            
                        }
                    }
                }else if(Require(proposal.Destination, out CItemHolder cItemHolder))
                {
                    if(Require(cItemHolder.HeldItem, out CRestrictedToolStorage storage))
                    {
                        string itemKey = storage.ItemKey.ToString();
                        if (!RestrictedItemTransfers.IsAllowed(itemKey, proposal.ItemType))
                        {
                            proposal.Status = ItemTransferStatus.Pruned;
                        }
                    }
                }

                if (proposal.Status == ItemTransferStatus.Pruned)
                {
                    proposal.PrunedBy = this;
                    //KitchenTraysPlus.Mod.LogInfo("Pruned!");
                }

                SetComponent(entity, proposal);
            }
        }

        internal static Entity GetOccupantAt(Vector3 position)
        {
            return Instance.GetOccupant(position);
        }

        internal static bool CanReachFrom(Vector3 from, Vector3 to)
        {
            return Instance.CanReach(from, to);
        }
    }
}
