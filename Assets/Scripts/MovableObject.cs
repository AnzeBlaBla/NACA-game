using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{

    public bool addRandomInitialVelocity = false;
    public Vector2 randomVelocityRange = new Vector2(-1f, 1f);

    public float moveForceAmount = 1f;

    public bool keepMomentum = true;

    bool held = false;
    Vector2 holdLocation = Vector2.zero;

    Vector2 oldMousePos = Vector2.zero;


    Rigidbody2D rb;
    GameInputs inputs;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (addRandomInitialVelocity)
        {
            rb.velocity = new Vector2(Random.Range(randomVelocityRange.x, randomVelocityRange.y), Random.Range(randomVelocityRange.x, randomVelocityRange.y));
        }


        inputs = new GameInputs();

        inputs.Interaction.Enable();

        inputs.Interaction.Tap.performed += ctx => StartHold(ctx);
        inputs.Interaction.Tap.canceled += ctx => StopHold(ctx);

    }

    void StartHold(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = MousePos();

        // get object under mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            held = true;

            holdLocation = mousePos - (Vector2)transform.position;
        }
    }

    void StopHold(InputAction.CallbackContext ctx)
    {
        held = false;

        if (!keepMomentum)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Destroy()
    {
        inputs.Interaction.Disable();
    }

    void FixedUpdate()
    {
        if (held)
        {
            //rb.MovePosition(MousePos() - holdLocation);

            Vector2 desiredPosition = MousePos() - holdLocation;

            rb.AddForce((desiredPosition - (Vector2)transform.position) * moveForceAmount, ForceMode2D.Force);
            oldMousePos = MousePos();
        }
    }

    Vector2 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}
