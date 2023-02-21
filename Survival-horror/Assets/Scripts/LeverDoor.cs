using UnityEngine;

public class LeverDoor : ConnectedObject
{
    [SerializeField] private Animator animator;
    public override void Execute()
    {
        animator.enabled = true;
    }
}