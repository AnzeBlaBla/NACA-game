using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockDoor : MonoBehaviour
{
    ClickableObject clickableObject;
    private Animator animator;

    private bool isLocked = true;
    void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();
        clickableObject.onClick += OnClick;

        animator = GetComponent<Animator>();
    }

    public void openDoor(){
        animator.SetTrigger("TrOpenAirLock");
    }


    // on click function
    public void OnClick()
    {
        Debug.Log("AirLockDoor clicked");
        if(isLocked)
        {
            this.openDoor();
        }
        else
        {
            // play sound
            // open door
        }
    }
}
