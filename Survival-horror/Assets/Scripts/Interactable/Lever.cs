using UnityEngine;

namespace Interactable
{
    public class Lever : Interactable
    {
        [SerializeField] private bool isWorking = false;
        [SerializeField] private string description = "Pull the lever";
        [SerializeField] private ConnectedObject connectedObject;
        [SerializeField] private Animator animator;
        
        public override void Interact(PlayerManager player)
        {
            if (isWorking)
            {
                animator.enabled = true;
            }
            else
            {
                player.PlayerInteraction.StartErrorTextAnimation("Lever is broken");
            }
        }

        public override string GetDescription()
        {
            return description;
        }

        public void ExecuteConnectedObject()
        {
            connectedObject.Execute();
        }
    }
}
