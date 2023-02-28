using System;
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

        public Transform secretWall;

        public string ignitedDescription = "";
        public string puttedOutDescription = "";

        private void Awake()
        {
            secretWall = FindObjectOfType<SecretWall>().transform;
        }

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
            
            secretWall.GetComponent<SecretWall>().CheckAllTorches();
        }

        public override string GetDescription()
        {
            return isIgnited ? puttedOutDescription : ignitedDescription;
        }
    }
}
