using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
    public GameObject background;
    public AnimationClip backgroundAnimation;
    Animation animation;

    public GameObject filmCanvas;
    public GameObject endGameCanvas;

    private IEnumerator WaitForAnimation ( Animation animation )
    {
        do
        {
            yield return null;
        } while ( animation.isPlaying );
    }

    private IEnumerator runAnimation ( Animation animation )
    {
        animation.AddClip( backgroundAnimation, "backgroundAnimation" );
        animation.Play( "backgroundAnimation" );
        yield return StartCoroutine( WaitForAnimation( animation ) );
        filmCanvas.SetActive( false );
        endGameCanvas.SetActive( true );
    }

    // Start is called before the first frame update
    void Start()
    {
        filmCanvas.SetActive( true );
        endGameCanvas.SetActive( false );
        animation = background.GetComponent<Animation>();
        StartCoroutine(runAnimation(background.GetComponent<Animation>()));
    }
}
