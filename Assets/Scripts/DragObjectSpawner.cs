using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class DragObjectSpawner : MonoBehaviour
{
    GameInputs inputs;

    GameObject held;

    public Sprite emptySprite;
    public Sprite fullSprite;

    public GameObject toSpawn;
    public string useItem;
    public float useAmount = 1f;

    void Start()
    {
        inputs = new GameInputs();
        inputs.Interaction.Enable();
        inputs.Interaction.Tap.performed += ctx => SpawnObject(ctx);
        inputs.Interaction.Tap.canceled += ctx => StopHold(ctx);

        StorageManager.Instance.onUpdate += SetSprite;
        SetSprite();
    }

    void SetSprite()
    {
        if (StorageManager.Instance.GetData(useItem) > 0)
        {
            GetComponent<SpriteRenderer>().sprite = fullSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = emptySprite;
        }
    }
    void SpawnObject(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos = PointerPos();

        // get object under mouse
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // if hit this or child
        if (hit.collider != null && (hit.collider.gameObject == gameObject || hit.collider.gameObject.transform.IsChildOf(transform)))
        {
            if (StorageManager.Instance.GetData(useItem) > 0)
            {
                held = Instantiate(toSpawn, mousePos, Quaternion.identity);
                StorageManager.Instance.ChangeData(useItem, useAmount * -1);
            }
        }
    }

    void StopHold(InputAction.CallbackContext ctx)
    {
        if (held != null)
        {
            Destroy(held);

            StorageManager.Instance.ChangeData(useItem, useAmount);
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
        if(StorageManager.Instance != null)
            StorageManager.Instance.onUpdate -= SetSprite;

        inputs.Interaction.Disable();
    }

    Vector2 PointerPos()
    {
        return Camera.main.ScreenToWorldPoint(inputs.Interaction.PointerPosition.ReadValue<Vector2>());
    }
}
