using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour
{
    ClickableObject clickableObject;
    public Transform attachPosition;

    public GameObject curtain;


    public Vector2 pushForce;
    void Awake()
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
            if (AstronautManager.Instance.data.bladder > 10f)
            {
                StartCoroutine(RunAutoToilet());
            }
        }
    }

    IEnumerator RunAutoToilet()
    {
        AttachPlayer();

        Animation anim = curtain.GetComponent<Animation>();
        anim.Play();

        yield return new WaitForSeconds(anim.clip.length);

        StorageManager.Instance.data.unfilteredWater += AstronautManager.Instance.data.bladder;
        AstronautManager.Instance.data.bladder = 0f;

        DetachPlayer();
    }

    public void OnClick()
    {

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
    }

    private void OnDestroy()
    {
        clickableObject.onClick -= OnClick;

        if (playerAttached)
        {
            DetachPlayer(false);
        }
    }
}
