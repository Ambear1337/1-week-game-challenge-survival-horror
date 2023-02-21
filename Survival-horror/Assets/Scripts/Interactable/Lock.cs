using UnityEngine;

public class Lock : Interactable.Interactable
{
    public string description = " ";
    public ConnectedObject connectedObject;

    public override void Interact(PlayerManager player)
    {
        connectedObject.Execute();

        Destroy(gameObject);
    }

    public override string GetDescription()
    {
        return description;
    }
}