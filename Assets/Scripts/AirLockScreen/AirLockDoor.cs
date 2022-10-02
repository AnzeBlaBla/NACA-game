using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockDoor : MonoBehaviour
{
    ClickableObject clickableObject;
    private Animator animator;

    private bool isLocked = true;
    private bool isAnimating = false;
    void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();
        clickableObject.onClick += OnClick;

        animator = GetComponent<Animator>();
    }

    IEnumerator openDoor(){
        isAnimating = true;
        animator.SetTrigger("TrOpenAirLock");
        yield return new WaitForSeconds(4);
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut();
        yield return StartCoroutine(AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().PlayAnimation("minimizing"));
        onAnimationComplete();
        isAnimating = false;
    }


     void onAnimationComplete()
     {
         ScreenManager.Instance.LoadScreen("OutsideAirLockScreen");
         ScreenManager.Instance.UnloadScreen(ScreenManager.Instance.currentScreenIndex);
     }


    // on click function
    public void OnClick()
    {
        if(isLocked)
        {
            if(!isAnimating)
            {
                StartCoroutine(openDoor());
            }
        }
    }
}
