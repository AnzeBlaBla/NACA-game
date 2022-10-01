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
}
