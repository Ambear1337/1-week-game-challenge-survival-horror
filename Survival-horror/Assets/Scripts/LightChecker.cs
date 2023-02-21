using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChecker : MonoBehaviour
{
    [SerializeField] private float lightDistance;
    [SerializeField] private LayerMask playerMask;

    private Transform player;
    private PlayerController playerController;

    private void Update()
    {
        if (!player) return;

        var position = transform.position;
        
        Debug.DrawRay(position, player.position - position, Color.yellow);
        
        if (!Physics.Raycast(transform.position, player.position - transform.position, out var hit, lightDistance)) return;
        
        if (hit.collider.transform == player)
        {
            playerController.isInLight = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        player = other.transform;
        playerController = player.GetComponent<PlayerManager>().PlayerController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || player == null) return;
        
        playerController.isInLight = false;
        player = null;
    }
}
