using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideAirLockDoor : MonoBehaviour
{
    public ScreenItem insideScreenItem;
    
    ClickableObject clickableObject;
    private Animator animator;

    private bool isAnimating = false;
   void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();
        clickableObject.onClick += OnClick;

        animator = GetComponent<Animator>();
    }

    void Start(){
        if(ScreenManager.Instance.isGoingInsideOrOutside){
            ScreenManager.Instance.isGoingInsideOrOutside = false;
            ScreenManager.Instance.DisableButtons();
            StartCoroutine(this.openDoor(true));
        }
    }

    IEnumerator openDoor(bool goingOut){
        if(!isAnimating)
        {
            isAnimating = true;
            AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut();
            animator.SetTrigger("TrOpenAirLock");
            yield return new WaitForSeconds(4);
            yield return StartCoroutine(goingOut ? goOut() : goIn());
            if(goingOut){
                animator.SetTrigger("TrCloseAirLock");
                changeRendererLayout(0);
                yield return new WaitForSeconds(4);
            }
            isAnimating = false;
        }
    }

    IEnumerator goIn(){
        yield return StartCoroutine(AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().PlayAnimation("minimizing"));
    }

    IEnumerator goOut(){
        yield return StartCoroutine(AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().PlayAnimation("maximizing"));
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().unlockAstronaut();
    }

    void changeRendererLayout(int index){
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = index;
    }

    void OnClick(){
        if(!isAnimating)
        {
            StartCoroutine(goInside());
        }
    }

    IEnumerator goInside(){
        yield return StartCoroutine(openDoor(false));
        ScreenManager.Instance.EnableButtons();
        ScreenManager.Instance.isGoingInsideOrOutside = true;
        ScreenManager.Instance.LoadScreen(insideScreenItem);
    }

}
