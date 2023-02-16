using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;

    public TMPro.TextMeshProUGUI interactionText;

    [SerializeField] private Camera cam;

    Interactable interactable;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (!hit.collider.isTrigger)
            {
                interactable = hit.collider.GetComponent<Interactable>();

                if (interactable != null)
                {
                    Debug.DrawRay(ray.origin, ray.direction, Color.green);
                    interactionText.text = interactable.GetDescription();
                    successfulHit = true;
                }
            }
        }

        if (!successfulHit)
        {
            interactionText.text = "";
            interactionText.color = Color.white;
        }
    }

    public void TryToInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && interactable != null)
        {
            switch (interactable.interactionType)
            {
                case Interactable.InteractionType.Click:
                    interactable.Interact(GetComponent<PlayerStats>());
                    break;
                case Interactable.InteractionType.Hold:
                    interactable.Interact(GetComponent<PlayerStats>());
                    break;
                default:
                    throw new System.Exception("Unsupported type of interactable.");
            }
        }
        else
        {
            return;
        }
    }
}
