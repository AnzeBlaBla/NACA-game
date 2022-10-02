using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClickableObject))]
public class FitnessDevice : MonoBehaviour
{
    ClickableObject clickableObject;

    public Transform attachPosition;
    public Vector2 pushForce;
    public GameObject tapButton;

    public float followDistance = 1.5f;

    public Vector3 runningPosition = new Vector3(0.5f, 0, 0);

    void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();

        // clickableObject.onClick += OnClick;
    }

    private void Start()
    {
        tapButton.SetActive(false);
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

        astronaut.transform.SetParent(transform);

        astronaut.transform.position = attachPosition.position;

        tapButton.SetActive(true);
    }

    void DetachPlayer(bool applyForce = true)
    {
        playerAttached = false;
        
        astronaut.GetComponent<MovableObject>().enabled = true;
        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();
        
        rb.isKinematic = false;

        if (applyForce)
        {
            rb.AddForce(pushForce, ForceMode2D.Impulse);
        }

        astronaut = null;

        astronaut.transform.SetParent(null);

        tapButton.SetActive(false);
    }

    void OnClick()
    {
        if (playerAttached)
        {
            DetachPlayer();
        }
    }

    public void OnActionTap()
    {
        AstronautManager.Instance.ChangeStat("fitness", 0.5f);
        astronaut.transform.position = astronaut.transform.position + runningPosition;
    }

    void Update()
    {
        if (playerAttached)
        {
            astronaut.transform.position = astronaut.transform.position - (runningPosition / 60);
            float distance = Mathf.Abs(astronaut.transform.position.x - attachPosition.position.x);
            if(distance > followDistance)
            {
                DetachPlayer();
            }
        }
    }

    private void OnDestroy()
    {
        // clickableObject.onClick -= OnClick;

        if (playerAttached)
        {
            DetachPlayer(false);
        }
    }
}
