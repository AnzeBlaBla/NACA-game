using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : Singleton<ScreenManager>
{
    public GameObject nextButton;
    public GameObject backButton;

    public List<ScreenItem> screens;
    
    public int currentScreenIndex = 0;
    public ScreenItem oldScreen;

    public Image background;
    public TextMeshProUGUI title;

    public bool isGoingInsideOrOutside = false;

    [SerializeField] private GameObject sceneSwitchSound;

    public void Start()
    {
        // Unload all screens
        for (int i = 0; i < screens.Count; i++)
        {
            UnloadScreenByIndex(i);
        }

        // Load the first screen
        LoadScreen(screens[currentScreenIndex]);
    }


    public void NextScreen()
    {
        sceneSwitchSound.GetComponent<AudioSource>().Play();

        int index = currentScreenIndex + 1;
        if (index >= screens.Count)
        {
            index = 0;
        }

        currentScreenIndex = index;

        LoadScreen(screens[index]);
    }

    public void PreviousScreen()
    {
        sceneSwitchSound.GetComponent<AudioSource>().Play();

        int index = currentScreenIndex - 1;
        if (index < 0)
        {
            index = screens.Count - 1;
        }

        currentScreenIndex = index;

        LoadScreen(screens[index]);
    }

    public void LoadScreen(ScreenItem screen)
    {
        SceneManager.LoadScene(screen.sceneName, LoadSceneMode.Additive);
        title.text = screen.displayName;

        if (screen.backgroundImage != null)
        {
            background.gameObject.SetActive(true);
            background.sprite = screen.backgroundImage;
        }
        else
        {
            background.gameObject.SetActive(false);
        }

        AstronautManager.Instance.gameObject.GetComponent<MovableObject>().ResetPosition();

        if (oldScreen != null)
        {
            UnloadScreenWithName(oldScreen.sceneName);
        }

        oldScreen = screen;
    }

    public void UnloadScreenByIndex(int index)
    {
        this.UnloadScreenWithName(screens[index].sceneName);
    }
    public void UnloadScreenWithName(string name)
    {
        try
        {
            SceneManager.UnloadSceneAsync(name);
        }
        catch { }

    }

    public void EnableButtons()
    {
        nextButton.SetActive(true);
        backButton.SetActive(true);
    }

    public void DisableButtons()
    {
        nextButton.SetActive(false);
        backButton.SetActive(false);
    }
}
