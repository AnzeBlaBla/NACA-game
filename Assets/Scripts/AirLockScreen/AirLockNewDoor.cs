using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockNewDoor : MonoBehaviour
{
    public ScreenItem screenToGoTo;

    public bool isItOutside = false;

    private bool isAnimating = false;

    void Start(){
        if(ScreenManager.Instance.isGoingInsideOrOutside){
            ScreenManager.Instance.isGoingInsideOrOutside = false;
            if(isItOutside){
                ScreenManager.Instance.DisableButtons();
            }else{
                ScreenManager.Instance.EnableButtons();
            }
            this.isAnimating = true;
            StartCoroutine(this.PushAstronaut());
        }
    }

    IEnumerator PushAstronaut(){
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut(transform.position);
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().unlockAstronaut();
        AstronautManager.Instance.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 100f));

        yield return new WaitForSeconds(1);
        isAnimating = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Astronaut") && !isAnimating)
        {
            GoToScreen();
        }
    }

    void GoToScreen(){
        ScreenManager.Instance.isGoingInsideOrOutside = true;
        ScreenManager.Instance.DisableButtons();
        AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut(transform.position);
        ScreenManager.Instance.LoadScreen(screenToGoTo);
    }
}
