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

    IEnumerator openDoor(){
        animator.SetTrigger("TrOpenAirLock");
        yield return new WaitForSeconds(3);
        onAnimationComplete();
    }


     void onAnimationComplete()
     {
         ScreenManager.Instance.LoadScreen("OutsideAirLockScreen", Color.black);
         ScreenManager.Instance.UnloadScreen(ScreenManager.Instance.currentScreenIndex);
     }


    // on click function
    public void OnClick()
    {
        Debug.Log("AirLockDoor clicked");
        if(isLocked)
        {
            StartCoroutine(this.openDoor());
        }
        else
        {
            // play sound
            // open door
        }
    }
}
