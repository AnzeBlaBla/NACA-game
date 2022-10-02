using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWarning : MonoBehaviour
{
    public GameObject warningOverlay;

    public float foodThreshold = 5f;
    public float waterThreshold = 10f;

    void Start()
    {
        AstronautManager.Instance.onUpdate += UpdateOverlay;
        UpdateOverlay();


    }

    void UpdateOverlay()
    {
        if (AstronautManager.Instance.data.food < foodThreshold || AstronautManager.Instance.data.water < waterThreshold)
        {
            warningOverlay.SetActive(true);
        }
        else
        {
            warningOverlay.SetActive(false);
        }
    }
}
