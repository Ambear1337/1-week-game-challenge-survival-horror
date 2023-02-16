using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    [SerializeField] GameObject equippedItem;
    [SerializeField] string description = " ";

    public override void Interact(PlayerStats player)
    {
        player.Equip(equippedItem);
        Destroy(gameObject);
    }

    public override string GetDescription()
    {
        return description;
    }
}
