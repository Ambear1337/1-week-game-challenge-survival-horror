using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace Interactable
{
    public class Torch : Interactable
    {
        public bool isIgnited;
        public Transform light;

        public string ignitedDescription = "";
        public string puttedOutDescription = "";
        
        public override void Interact(PlayerManager player)
        {
            if (isIgnited)
            {
                isIgnited = false;
                light.gameObject.SetActive(false);
            }
            else
            {
                isIgnited = true;
                light.gameObject.SetActive(true);
            }
        }

        public override string GetDescription()
        {
            if (isIgnited)
            {
                return puttedOutDescription;
            }
            else
            {
                return ignitedDescription;
            }
        }
    }
}
