using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public string tutorialCanvasId;

    private void Awake()
    {
        string playerPrefsKey = "tutorial_" + tutorialCanvasId;
        if (PlayerPrefs.GetInt(playerPrefsKey, 0) == 1)
        {
            gameObject.SetActive(false);
        }

        PlayerPrefs.SetInt(playerPrefsKey, 1);
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
