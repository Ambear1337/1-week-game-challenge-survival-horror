using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEquippedItem : EquippedItem
{
    [SerializeField] private GameObject pickupItem;

    public override void Use()
    {
        return;
    }

    public override void Drop()
    {
        Instantiate(pickupItem, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public override void DestroyItem()
    {
        Destroy(gameObject);
    }
}
