using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private CharacterController controller;

    private bool isCrouching = false;
    private bool canUncrouch = true;

    public bool isSprinting = false;
    private bool canSprint = true;

    private float rotY;
    private float rotX;

    public bool isInLight = false;
    public float noiseValue;

    public Camera cam;

    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float mouseSensitivity = 1f;

    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float moveSpeed = 3f;

    [SerializeField] private Vector3 crouchScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;

    [SerializeField] private InputHandler input;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        controller = GetComponent<CharacterController>();

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
        var moveDirection = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0) *
                            new Vector3(input.inputDirection.x, 0, input.inputDirection.y).normalized;
        var velocity = moveDirection * (moveSpeed * Time.deltaTime);

        if (!controller.isGrounded)
        {
            velocity += (Physics.gravity * Time.deltaTime);
        }

        // Changing noise value related to player state value
        if (input.inputDirection != Vector2.zero)
        {
            noiseValue = isCrouching switch
            {
                false when isSprinting == false => 0.6f,
                true when isSprinting == false => 0.3f,
                false when isSprinting == true => 1f,
                _ => 0.6f
            };
        }
        else
        {
            noiseValue = 0f;
        }

        controller.Move(velocity);
    }

    //arttymen
    private void Rotation()
    {
        rotY += input.mouseInput.x * mouseSensitivity * Time.deltaTime;
        rotX -= input.mouseInput.y * mouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -80f, 80f);

        var desiredPlayerRotation =
            Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotY, 0f), smoothTime);
        var desiredCameraRotation =
            Quaternion.Slerp(cam.transform.localRotation, Quaternion.Euler(rotX, 0f, 0f), smoothTime);

        transform.rotation = desiredPlayerRotation;
        cam.transform.localRotation = desiredCameraRotation;
    }


    public void UseItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && playerManager.PlayerStats.equippedItem != null)
        {
            playerManager.PlayerStats.equippedItem.Use();
        }
    }

    public void DropItem(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && playerManager.PlayerStats.equippedItem != null)
        {
            playerManager.PlayerStats.equippedItem.Drop();
        }
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        switch (ctx.performed)
        {
            case true when isCrouching == false:
                if (isSprinting == true)
                {
                    isSprinting = false;
                }

                isCrouching = true;
                moveSpeed = crouchSpeed;
                noiseValue = 0.3f;

                transform.localScale = crouchScale;
                break;
            case true when isCrouching == true && canUncrouch:
                isCrouching = false;
                moveSpeed = walkSpeed;
                noiseValue = 0.6f;

                transform.localScale = normalScale;
                break;
        }
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canSprint == true && isCrouching == false)
        {
            isSprinting = true;
            moveSpeed = runSpeed;
            noiseValue = 1f;

            playerManager.PlayerStats.StartSprint();
        }
        else if (ctx.canceled && isCrouching == false)
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
            noiseValue = 1f;

            playerManager.PlayerStats.StopSprint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            canUncrouch = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("HidingSpot"))
        {
            canUncrouch = true;
        }
    }
}