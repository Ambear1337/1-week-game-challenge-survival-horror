using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class CogPlace : Interactable
    {
        public string description = " ";
        public ConnectedObject connectedObject;

        private bool canInteract = true;
        
        public override void Interact(PlayerManager player)
        {
            if (!canInteract) return;
            
            connectedObject.gameObject.SetActive(true);

            connectedObject.Execute();

            canInteract = false;
        }

        public override string GetDescription()
        {
            if (canInteract)
            {
                return description;
            }
            else
            {
                return " ";
            }
        }
    }
}