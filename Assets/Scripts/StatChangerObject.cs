using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StatChangerObject : MonoBehaviour
{
    public string statName;
    public float statChange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
        if (collision.gameObject.CompareTag("Astronaut"))
        {
            // if this stat is not full
            if (AstronautManager.Instance.GetStat(statName) < 100)
            {
                AstronautManager.Instance.ChangeStat(statName, statChange);
                Destroy(gameObject);
            }
        }
    }

}
