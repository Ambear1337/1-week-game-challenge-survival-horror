using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;

    [SerializeField] Transform equippedItemTransform;
    [SerializeField] GameObject equippedItemGO;
    [SerializeField] Camera cam;

    public EquippedItem equippedItem;

    public void GetDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
        }
    }

    public void Equip(GameObject equipItem)
    {
        equippedItemGO = Instantiate(equipItem, equippedItemTransform.position, equippedItemTransform.rotation, cam.transform);
        equippedItem = equippedItemGO.GetComponent<EquippedItem>();
    }
}
