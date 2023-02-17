using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField]
    private LayerMask grabbableLayer;

    private Transform defaultEnergyParent;

    private Vector3 defaultEnergyLocalPosition;
    private Vector3 currentSmoothVelocity = Vector3.zero;

    public GameObject grabObject;

    private Rigidbody grabObjectRb;
    private Collider grabObjectCollider;

    float grabStrength;

    [SerializeField]
    private float grabStrengthMultiplier = 2.0f;

    [SerializeField]
    float grapDistance = 3.0f;

    [SerializeField]
    float grabRange = 3.0f;

    [SerializeField]
    private Transform energy;

    [SerializeField]
    private Camera followCam;

    private void Update()
    {
        if (grabObject)
        {
            grabStrength = grabStrengthMultiplier + grabObjectRb.mass;

            Vector3 desiredPosition = (energy.transform.position - grabObject.transform.position) * grabStrength;
            grabObjectRb.AddForce(desiredPosition);

            if (Vector3.Distance(energy.transform.position, grabObject.transform.position) >= grapDistance)
            {
                DropObject();
            }
        }
    }

    public void DragDropObject(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !grabObject)
        {
            GrabObject();
        }

        if (ctx.canceled && grabObject)
        {
            DropObject();
        }
    }

    void GrabObject()
    {
        Ray ray = new Ray(followCam.transform.position, followCam.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, grabRange, grabbableLayer))
        {
            if (grabObjectRb)
            {
                return;
            }
            else
            {
                defaultEnergyLocalPosition = energy.localPosition;
                defaultEnergyParent = energy.transform.parent;
                energy.transform.parent = followCam.transform;

                grabObjectRb = hit.rigidbody;
                grabObjectCollider = hit.collider;
                grabObject = hit.collider.gameObject;

                energy.transform.position = hit.point;
                grabObjectRb.drag = 10.0f;
            }
            Debug.DrawLine(ray.origin, hit.point, Color.green, 3f);
        }
    }

    void DropObject()
    {
        grabObject.transform.parent = null;
        grabObjectRb.drag = default;

        grabObjectRb = null;
        grabObjectCollider = null;
        grabObject = null;

        energy.transform.parent = defaultEnergyParent;
        energy.transform.localPosition = defaultEnergyLocalPosition;
    }
}
