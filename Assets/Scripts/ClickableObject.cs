using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ClickableObject : MonoBehaviour
{
    GameInputs inputs;
    public Action onClick;

    

    void Start()
    {

        inputs = new GameInputs();

        inputs.Interaction.Enable();

        inputs.Interaction.Tap.performed += ctx => CheckClick(ctx);

    }

    void CheckClick(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = MousePos();

        // get object under mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // if hit this or child
        if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject.transform.IsChildOf(transform)))
        { 
            onClick?.Invoke();
        }
    }

    void OnDestroy()
    {
        inputs.Interaction.Disable();
    }

    Vector2 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
