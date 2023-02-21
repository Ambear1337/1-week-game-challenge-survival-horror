using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private PlayerManager playerManager;

    [Header("Health")] private Color healthBarDefaultColor;
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private Image healthBar;

    [SerializeField] private float screamerFOV = 30f;
    [SerializeField] private float normalFOV = 60f;

    [SerializeField] Transform equippedItemTransform;
    [SerializeField] GameObject equippedItemGO;

    public EquippedItem equippedItem;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        healthBarDefaultColor = healthBar.color;
    }

    public void GetDamage(int damage, Transform enemy)
    {
        health -= damage;

        if (health < 0)
        {
            health = 0;
            Death();
        }

        UpdateHealthBar();

        playerManager.PlayerInput.enabled = false;
        playerManager.PlayerController.enabled = false;
        playerManager.PlayerController.cam.fieldOfView = screamerFOV;

        StartCoroutine(ScreamerCoroutine(enemy));
    }

    public void ReleasePlayer()
    {
        playerManager.PlayerInput.enabled = true;
        playerManager.PlayerController.enabled = true;
        playerManager.PlayerController.cam.fieldOfView = normalFOV;

        StartCoroutine(RegenerateHealth());
    }

    private static void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ScreamerCoroutine(Transform enemyFace)
    {
        while (playerManager.PlayerInput.enabled == false)
        {
            playerManager.PlayerController.cam.transform.LookAt(enemyFace);

            yield return null;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (health < maxHealth)
        {
            yield return new WaitForSeconds(1f);

            health += 5;

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        var healthBarNewColor = new Color(healthBarDefaultColor.r, healthBarDefaultColor.g, healthBarDefaultColor.b,
            (maxHealth * 0.01f - health * 0.01f) / 2);
        healthBar.color = healthBarNewColor;
    }

    public void Equip(GameObject equipItem)
    {
        equippedItemGO = Instantiate(equipItem, equippedItemTransform.position, equippedItemTransform.rotation,
            playerManager.PlayerController.cam.transform);
        equippedItem = equippedItemGO.GetComponent<EquippedItem>();
    }
}