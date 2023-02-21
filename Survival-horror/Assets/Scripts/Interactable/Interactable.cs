using UnityEngine;

namespace Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        public enum InteractionType
        {
            Click,
            Hold
        }

        public enum ItemType
        {
            Null,
            Lockpick
        }

        public ItemType itemType;

        public InteractionType interactionType;

        public abstract string GetDescription();

        public abstract void Interact(PlayerManager player);
    }
}
