using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private PlayerManager playerManager;

    [Header("Health")] private Color healthBarDefaultColor;
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private int healthRegenerationRate = 5;

    [SerializeField] private int stamina = 100;
    [SerializeField] private int maxStamina = 100;

    [SerializeField] private int staminaConsumingRate = 10;
    [SerializeField] private int staminaRegenerationRate = 5;

    [SerializeField] private Image healthBar;

    [SerializeField] private float screamerFOV = 30f;
    [SerializeField] private float normalFOV = 60f;

    [SerializeField] Transform equippedItemTransform;
    [SerializeField] GameObject equippedItemGO;

    private Camera cam;

    public UnityEvent OnChaseStateChanged;

    private bool isChased = false;
    
    public bool IsChased
    {
        get { return isChased; }
        set
        {
            isChased = value;
            OnChaseStateChanged.Invoke();
        }
    }

    public EquippedItem equippedItem;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        cam = playerManager.PlayerController.cam;
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
        cam.fieldOfView = screamerFOV;

        StartCoroutine(ScreamerCoroutine(enemy));
    }

    public void ReleasePlayer()
    {
        playerManager.PlayerInput.enabled = true;
        playerManager.PlayerController.enabled = true;
        cam.fieldOfView = normalFOV;

        StartCoroutine(RegenerateHealth());
    }

    private static void Death()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator ScreamerCoroutine(Transform enemyFace)
    {
        while (playerManager.PlayerInput.enabled == false)
        {
            cam.transform.LookAt(enemyFace);

            yield return null;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (health < maxHealth)
        {
            yield return new WaitForSeconds(1f);

            health += healthRegenerationRate;

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            UpdateHealthBar();
        }
    }
    
    private IEnumerator RegenerateStamina()
    {
        while (stamina < maxStamina && !playerManager.PlayerController.isSprinting)
        {
            yield return new WaitForSeconds(1f);

            stamina += staminaRegenerationRate;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            //Update stamina bar
        }
    }

    public void StartSprint()
    {
        StopCoroutine((RegenerateStamina()));
        StartCoroutine(ConsumeStamina());
    }

    public void StopSprint()
    {
        StopCoroutine(ConsumeStamina());
        StartCoroutine(RegenerateStamina());
    }
    private IEnumerator ConsumeStamina()
    {
        while (stamina > 0 && playerManager.PlayerController.isSprinting)
        {
            yield return new WaitForSeconds(1f);

            stamina -= staminaConsumingRate;

            if (stamina < 0)
            {
                stamina = 0;
            }
        }
        
        //Update stamina bar
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

    public void DestroyEquippedItem()
    {
        
    }
}