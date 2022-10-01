using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class DragObjectSpawner : MonoBehaviour
{
    GameInputs inputs;

    GameObject held;

    public GameObject toSpawn;

    void Start()
    {

        inputs = new GameInputs();
        inputs.Interaction.Enable();
        inputs.Interaction.Tap.performed += ctx => SpawnObject(ctx);
        inputs.Interaction.Tap.canceled += ctx => StopHold(ctx);

    }

    void SpawnObject(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = PointerPos();

        // get object under mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // if hit this or child
        if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject.transform.IsChildOf(transform)))
        {
            held = Instantiate(toSpawn, mousePos, Quaternion.identity);
        }
    }

    void StopHold(InputAction.CallbackContext ctx)
    {
        if (held != null)
        {
            Destroy(held);
        }
    }

    private void Update()
    {
        if (held != null)
        {
            held.transform.position = PointerPos();
        }
    }

    void OnDestroy()
    {
        inputs.Interaction.Disable();
    }

    Vector2 PointerPos()
    {
        return Camera.main.ScreenToWorldPoint(inputs.Interaction.PointerPosition.ReadValue<Vector2>());
    }
}
