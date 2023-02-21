using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private PlayerManager playerManager;

    private CharacterController controller;

    private bool isCrouching = false;
    private bool canUncrouch = true;

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

    [SerializeField] InputHandler input;

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