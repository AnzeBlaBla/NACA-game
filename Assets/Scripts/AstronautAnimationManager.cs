using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautAnimationManager : MonoBehaviour
{

    public Animation astronautAnimation;

    private bool isPlayingAnimation = false;
    public IEnumerator PlayAnimation(string animationName)
    {
        if (isPlayingAnimation)
        {
            yield break;
        }
        Debug.Log("Playing animation: " + animationName);
        isPlayingAnimation = true;
        astronautAnimation.Play(animationName);
        yield return StartCoroutine( WaitForAnimation( astronautAnimation ) );
        isPlayingAnimation = false;
    }

    private IEnumerator WaitForAnimation ( Animation animation )
    {
        do
        {
            yield return null;
        } while ( animation.isPlaying );
    }

    public void lockAstronaut(Vector3 position = default(Vector3)){

        GameObject astronaut = AstronautManager.Instance.gameObject;

        astronaut.GetComponent<MovableObject>().enabled = false;

        if(position != default(Vector3)){
            transform.position = position;
        }
    }

    public void unlockAstronaut()
    {
        GameObject astronaut = AstronautManager.Instance.gameObject;

        astronaut.GetComponent<MovableObject>().enabled = true;
        Rigidbody2D rb = astronaut.GetComponent<Rigidbody2D>();
        
        rb.isKinematic = false;
    }
}
