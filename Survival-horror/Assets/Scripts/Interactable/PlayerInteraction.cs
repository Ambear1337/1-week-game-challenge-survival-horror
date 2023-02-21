using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Interactable
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerManager playerManager;

        public float interactionDistance = 3f;

        public TMPro.TextMeshProUGUI InteractionText;

        [FormerlySerializedAs("ICantDoThisText")]
        public TMPro.TextMeshProUGUI errorText;

        [SerializeField] private Camera cam;

        Interactable interactable;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
        }

        // Update is called once per frame
        void Update()
        {
            var ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            var successfulHit = false;

            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                if (!hit.collider.isTrigger)
                {
                    interactable = hit.collider.GetComponent<Interactable>();

                    if (!interactable)
                    {
                        Debug.DrawRay(ray.origin, ray.direction, Color.green);
                        InteractionText.text = interactable.GetDescription();
                        successfulHit = true;
                    }
                }
            }

            if (!successfulHit)
            {
                InteractionText.text = "";
                InteractionText.color = Color.white;
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
            
            var animator = errorText.transform.GetComponent<Animator>();

            animator.enabled = true;
            animator.Play("Base Layer.ErrorTextAnim");
        }
    }
}