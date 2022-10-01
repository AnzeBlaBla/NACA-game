using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSceneManager : MonoBehaviour
{
    public GameObject background;

    private IEnumerator WaitForAnimation ( Animation animation )
{
    do
    {
        yield return null;
    } while ( animation.isPlaying );
}

    // Start is called before the first frame update
    void Start()
    {
        background.GetComponent<Animation>().Play();
        StartCoroutine ( WaitForAnimation ( background.GetComponent<Animation>() ) );
    }
}
