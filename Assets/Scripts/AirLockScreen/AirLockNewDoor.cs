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
        AstronautAnimationManager aam = AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>();

        GameObject astronaut = AstronautManager.Instance.gameObject;

        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();

        Vector2 oldVelocity = rb.velocity;
        rb.velocity = new Vector2(oldVelocity.x, Mathf.Abs(oldVelocity.y));

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
        ScreenManager.Instance.LoadScreen(screenToGoTo);
    }
}
