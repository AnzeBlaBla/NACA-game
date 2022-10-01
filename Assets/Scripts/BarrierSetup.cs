using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierSetup : MonoBehaviour
{
    public GameObject topRight;
    public GameObject bottomLeft;
    // Start is called before the first frame update
    void Awake()
    {
        topRight.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        bottomLeft.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
    }

    
}
