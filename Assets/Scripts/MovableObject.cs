using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovableObject : MonoBehaviour
{

    public bool addRandomInitialVelocity = false;
    public Vector2 randomVelocityRange = new Vector2(-1f, 1f);
    public Vector2 randomRotationalVelocityRange = new Vector2(-1f, 1f);

    public float forceMultiplier = 1f;
    public float torqueMultiplier = 1f;

    public bool keepMomentum = true;

    bool held = false;
    Vector2 holdLocation = Vector2.zero;

    Rigidbody2D rb;
    GameInputs inputs;
    Vector3 startingPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startingPosition = transform.position;
    }

    void Start()
    {
        inputs = new GameInputs();

        inputs.Interaction.Enable();

        inputs.Interaction.Tap.performed += ctx => StartHold(ctx);
        inputs.Interaction.Tap.canceled += ctx => StopHold(ctx);

        ResetPosition();

    }

    void StartHold(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = PointerPos();

        // get object under mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // if hit this or child
        if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject.transform.IsChildOf(transform)))
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

    void OnDestroy()
    {
        inputs.Interaction.Disable();
    }

    void FixedUpdate()
    {
        if (held)
        {
            //rb.MovePosition(MousePos() - holdLocation);

            Vector2 desiredPosition = PointerPos() - holdLocation;

            rb.AddForce((desiredPosition - (Vector2)transform.position) * forceMultiplier, ForceMode2D.Force);

        }
    }

    Vector2 PointerPos()
    {
        return Camera.main.ScreenToWorldPoint(inputs.Interaction.PointerPosition.ReadValue<Vector2>());
    }

    public void ResetPosition()
    {
        transform.position = startingPosition;
        transform.rotation = Quaternion.identity;

        if (addRandomInitialVelocity)
        {
            rb.velocity = new Vector2(Random.Range(randomVelocityRange.x, randomVelocityRange.y), Random.Range(randomVelocityRange.x, randomVelocityRange.y));
            rb.angularVelocity = Random.Range(randomRotationalVelocityRange.x, randomRotationalVelocityRange.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

        }
    }
}
