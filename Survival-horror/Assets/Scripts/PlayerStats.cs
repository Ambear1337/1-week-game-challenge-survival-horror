using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private float screamerFOV = 30f;
    [SerializeField] private float normalFOV = 60f;

    [SerializeField] Transform equippedItemTransform;
    [SerializeField] GameObject equippedItemGO;
    [SerializeField] Camera cam;

    public EquippedItem equippedItem;

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void GetDamage(int damage, Transform enemy)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
            Death();
        }

        controller.playerInput.enabled = false;
        controller.enabled = false;
        controller.cam.fieldOfView = screamerFOV;

        StartCoroutine(ScreamerCoroutine(enemy));
    }

    public void ReleasePlayer()
    {
        controller.enabled = true;
        controller.playerInput.enabled = true;
        controller.cam.fieldOfView = normalFOV;
    }

    private void Death()
    {

    }

    IEnumerator ScreamerCoroutine(Transform enemyFace)
    {
        while (controller.playerInput.enabled == false)
        {
            controller.cam.transform.LookAt(enemyFace);

            yield return null;
        }
    }

    public void Equip(GameObject equipItem)
    {
        equippedItemGO = Instantiate(equipItem, equippedItemTransform.position, equippedItemTransform.rotation, cam.transform);
        equippedItem = equippedItemGO.GetComponent<EquippedItem>();
    }
}
