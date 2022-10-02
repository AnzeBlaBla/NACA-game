using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockDoor : MonoBehaviour
{
    ClickableObject clickableObject;
    private Animator animator;
    public ScreenItem outsideScreen;

    private bool isAnimating = false;
    void Awake()
    {
        clickableObject = GetComponent<ClickableObject>();
        clickableObject.onClick += OnClick;

        animator = GetComponent<Animator>();
    }

    void Start(){
        if(ScreenManager.Instance.isGoingInsideOrOutside){
            ScreenManager.Instance.DisableButtons();
            ScreenManager.Instance.isGoingInsideOrOutside = false;
            changeRendererLayout(100);
            StartCoroutine(this.openDoor(true));
        }
    }

    IEnumerator openDoor(bool goingOut){
        if(!isAnimating)
        {
            isAnimating = true;
            AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut(transform.position);
            animator.SetTrigger("TrOpenAirLock");
            yield return new WaitForSeconds(2);
            yield return StartCoroutine(goingOut ? goOut() : goIn());
            if(goingOut){
                addForceToAstronaut();
                animator.SetTrigger("TrCloseAirLock");
                changeRendererLayout(0);
                yield return new WaitForSeconds(2);
            }
            ScreenManager.Instance.EnableButtons();
            isAnimating = false;
        }
    }

    void addForceToAstronaut(){
        AstronautManager.Instance.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 15f));
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
    // on click function
    public void OnClick()
    {
        if(!isAnimating)
        {
            ScreenManager.Instance.DisableButtons();
            StartCoroutine(goOutside());
        }
    }

    IEnumerator goOutside(){
        yield return StartCoroutine(openDoor(false));
        ScreenManager.Instance.DisableButtons();
        ScreenManager.Instance.isGoingInsideOrOutside = true;
        ScreenManager.Instance.LoadScreen(outsideScreen);
    }

    void OnDestory(){
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().unlockAstronaut();
    }
}
