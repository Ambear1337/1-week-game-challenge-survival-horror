using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    PlayerStats stats;

    private bool isInLight = true;

    Vector3 currentCameraVelocity;

    private float rotY;
    private float rotX;

    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float mouseSensitivity = 1f;

    [SerializeField] float moveSpeed = 3f;

    [SerializeField] InputHandler input;
    [SerializeField] Camera cam;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        stats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Movement();
    }

    private void LateUpdate()
    {
        Rotation();
    }

    private void Movement()
    {
        Vector3 moveDirection = Quaternion.Euler(0, cam.transform.eulerAngles.y ,0) * new Vector3(input.inputDirection.x, 0, input.inputDirection.y).normalized;
        Vector3 velocity = moveDirection * moveSpeed * Time.deltaTime;

        if (!controller.isGrounded)
        {
            velocity = velocity + (Physics.gravity * Time.deltaTime);
        }

        controller.Move(velocity);
    }

    //arttymen
    public void Rotation()
    {
        rotY += input.mouseInput.x * mouseSensitivity * Time.deltaTime;
        rotX -= input.mouseInput.y * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -80f, 80f);

        Quaternion desiredPlayerRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotY, 0f), smoothTime);
        Quaternion desiredCameraRotation = Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(rotX, 0f, 0f), smoothTime);

        transform.rotation = desiredPlayerRotation;
        cam.transform.localRotation = desiredCameraRotation;
    }

    public void UseItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && stats.equippedItem != null)
        {
            stats.equippedItem.Use();
        }
    }

    public void DropItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && stats.equippedItem != null)
        {
            stats.equippedItem.Drop();
        }
    }

    public bool IsInLight()
    {
        return isInLight;
    }
}
