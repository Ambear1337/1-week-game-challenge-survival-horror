using UnityEngine;
using UnityEngine.AI;

public class LockedDoor : ConnectedObject
{
    public Rigidbody rb;
    public NavMeshObstacle obstacle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    public override void Execute()
    {
        rb.freezeRotation = false;
        rb.isKinematic = false;
        obstacle.enabled = false;
    }
}
