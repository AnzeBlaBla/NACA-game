using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class StatChangerObject : MonoBehaviour
{
    public string statName;
    public float statChange;

    [SerializeField] private AudioSource sound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggered");
        if (collision.gameObject.CompareTag("Astronaut"))
        {
            // if this stat is not full
            if (AstronautManager.Instance.GetStat(statName) < 100)
            {
                AstronautManager.Instance.ChangeStat(statName, statChange);

                AudioSource.PlayClipAtPoint(sound.clip, transform.position);
                Destroy(gameObject);
            }
        }
    }

}
