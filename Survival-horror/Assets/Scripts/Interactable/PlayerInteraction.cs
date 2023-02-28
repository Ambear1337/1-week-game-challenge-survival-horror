using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Interactable
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerManager playerManager;

        public LayerMask interactableMask;

        public float interactionDistance = 3f;

        [FormerlySerializedAs("InteractionText")] public TMPro.TextMeshProUGUI interactionText;

        [FormerlySerializedAs("ICantDoThisText")]
        public TMPro.TextMeshProUGUI errorText;

        [SerializeField] private Camera cam;

        Interactable interactable;

        private Animator errorTextAnimator;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            errorTextAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            var ray = new Ray(cam.transform.position, cam.transform.forward);

            var successfulHit = false;

            if (Physics.Raycast(ray, out var hit, interactionDistance, interactableMask))
            {
                if (hit.collider.GetComponent<Interactable>())
                {
                    interactable = hit.collider.GetComponent<Interactable>();

                    interactionText.text = interactable.GetDescription();
                    successfulHit = true;
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
                switch (interactable.itemType)
                {
                    case Interactable.ItemType.Null:
                        
                        interactable.Interact(playerManager);
                        
                        break;
                    
                    case Interactable.ItemType.Lockpick:
                        
                        if (playerManager.PlayerStats.equippedItem != null &&
                            playerManager.PlayerStats.equippedItem.gameObject.name.StartsWith("Lockpick"))
                        {
                            interactable.Interact(playerManager);
                        }
                        else
                        {
                            StartErrorTextAnimation("I can't do this");
                            return;
                        }

                        break;
                    
                    case Interactable.ItemType.BlueCog:
                        
                        if (playerManager.PlayerStats.equippedItem != null &&
                           playerManager.PlayerStats.equippedItem.gameObject.name.StartsWith("CogBlue"))
                        {
                            interactable.Interact(playerManager);
                            playerManager.PlayerStats.equippedItem.DestroyItem();
                        }
                    
                        break;
                    
                    case Interactable.ItemType.YellowCog:
                        
                        if (playerManager.PlayerStats.equippedItem != null &&
                            playerManager.PlayerStats.equippedItem.gameObject.name.StartsWith("CogYellow"))
                        {
                            interactable.Interact(playerManager);
                            playerManager.PlayerStats.equippedItem.DestroyItem();
                        }
                        
                        break;
                    
                    case Interactable.ItemType.RedCog:
                        
                        if (playerManager.PlayerStats.equippedItem != null &&
                            playerManager.PlayerStats.equippedItem.gameObject.name.StartsWith("CogRed"))
                        {
                            interactable.Interact(playerManager);
                            playerManager.PlayerStats.equippedItem.DestroyItem();
                        }
                        
                        break;
                    case Interactable.ItemType.Lantern:
                        
                        if (playerManager.PlayerStats.equippedItem != null &&
                            playerManager.PlayerStats.equippedItem.gameObject.name.StartsWith("Lantern"))
                        {
                            interactable.Interact(playerManager);
                        }
                        
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

        public void StartErrorTextAnimation(string errorString)
        {
            errorText.text = errorString;
            
            StartCoroutine(ErrorTextMessage());
        }

        private IEnumerator ErrorTextMessage()
        {
            yield return new WaitForSeconds(2f);
            errorText.text = "";
        }
    }
}