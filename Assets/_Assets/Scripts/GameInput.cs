using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;




public class GameInput : MonoBehaviour
{

    private PlayerInputActions playerInputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //if (onInteractAction != null)
        //{
        //    onInteractAction(this, EventArgs.Empty);
        //}

        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        bool useNewInputSystem = true;
        Vector2 inputVector;

        if (useNewInputSystem)
        {
            inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        }
        else
        {
            inputVector = new Vector2(0, 0);

            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y = +1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x = -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x = +1;
            }
        }

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
