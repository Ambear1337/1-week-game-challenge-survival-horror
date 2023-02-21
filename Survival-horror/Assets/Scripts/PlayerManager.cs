using System.Collections;
using System.Collections.Generic;
using Interactable;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerGrab playerGrab;
    [SerializeField] private PlayerInput playerInput;

    public PlayerInteraction PlayerInteraction => playerInteraction;
    public PlayerStats PlayerStats => playerStats;
    public PlayerController PlayerController => playerController;
    public PlayerGrab PlayerGrab => playerGrab;
    public PlayerInput PlayerInput => playerInput;

    private void Awake()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
        playerGrab = GetComponent<PlayerGrab>();
        playerInput = GetComponent<PlayerInput>();
    }
}