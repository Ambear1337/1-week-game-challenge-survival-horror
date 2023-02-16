using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 inputDirection;
    public Vector2 mouseInput;

    public void CheckInputDirection(InputAction.CallbackContext ctx)
    {
        inputDirection = ctx.ReadValue<Vector2>();
    }

    public void CheckMouseInput(InputAction.CallbackContext ctx)
    {
        mouseInput = ctx.ReadValue<Vector2>();
    }
}
