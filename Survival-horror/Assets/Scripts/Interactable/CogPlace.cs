using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class CogPlace : Interactable
    {
        public string description = " ";
        public ConnectedObject connectedObject;
        
        public override void Interact(PlayerManager player)
        {
            connectedObject.gameObject.SetActive(true);
            connectedObject.Execute();
        }

        public override string GetDescription()
        {
            return description;
        }
    }
}