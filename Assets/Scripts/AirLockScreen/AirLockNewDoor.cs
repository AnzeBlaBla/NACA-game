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
        //AstronautAnimationManager aam = AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>();
        //aam.lockAstronaut(transform.position);
        //aam.unlockAstronaut();

        GameObject astronaut = AstronautManager.Instance.gameObject;

        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();

        Vector2 oldVelocity = rb.velocity;

        Debug.Log(oldVelocity);

        astronaut.transform.position = transform.position;

        //rb.velocity = new Vector2(oldVelocity.x, oldVelocity.y * -1f);
        rb.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 5f));

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
        //AstronautManager.Instance.gameObject.GetComponent<AstronautAnimationManager>().lockAstronaut(transform.position);
        ScreenManager.Instance.LoadScreen(screenToGoTo);
    }
}
