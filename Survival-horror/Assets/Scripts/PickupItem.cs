using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable.Interactable
{
    [SerializeField] private GameObject equippedItem;
    [SerializeField] private string description = " ";

    public override void Interact(PlayerManager player)
    {
        player.PlayerStats.Equip(equippedItem);
        Destroy(gameObject);
    }

    public override string GetDescription()
    {
        return description;
    }
}