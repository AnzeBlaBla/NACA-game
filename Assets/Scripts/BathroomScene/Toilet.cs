using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    ClickableObject clickableObject;
    public Transform attachPosition;

    public Vector2 pushForce;
    void Start()
    {
        clickableObject = GetComponent<ClickableObject>();

        clickableObject.onClick += OnClick;
    }

    bool playerAttached = false;
    GameObject astronaut;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerAttached)
            return;

        if (other.CompareTag("Astronaut"))
        {
            AttachPlayer();
        }
    }


    void AttachPlayer()
    {
        playerAttached = true;

        astronaut = AstronautManager.Instance.gameObject;


        astronaut.GetComponent<MovableObject>().enabled = false;
        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        astronaut.transform.rotation = Quaternion.identity;

        astronaut.transform.position = attachPosition.position + new Vector3(0, 0.5f, 0);
    }

    void DetachPlayer()
    {
        playerAttached = false;

        astronaut.GetComponent<MovableObject>().enabled = true;
        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();
        
        rb.isKinematic = false;

        rb.AddForce(pushForce, ForceMode2D.Impulse);

        astronaut = null;
    }

    // Update is called once per frame
    void OnClick()
    {
        if (playerAttached)
        {
            DetachPlayer();
        }
    }
}
