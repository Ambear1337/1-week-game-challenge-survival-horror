using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class PickupItem : Interactable
    {
        [SerializeField] private GameObject equippedItem;
        [SerializeField] private string description = " ";

        public override void Interact(PlayerManager player)
        {
            if (player.PlayerStats.equippedItem != null) return;
            
            player.PlayerStats.Equip(equippedItem);
            Destroy(gameObject);
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}