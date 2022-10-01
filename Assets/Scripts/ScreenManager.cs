using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
    public List<ScreenItem> screens;
    public int currentScreenIndex = 0;

    public Image background;

    public void Start()
    {
        // Unload all screens
        for (int i = 0; i < screens.Count; i++)
        {
            UnloadScreen(i);
        }

        // Load the first screen
        LoadCurrentScreen();
    }


    public void NextScreen()
    {
        int oldIndex = currentScreenIndex;
        currentScreenIndex++;
        if (currentScreenIndex >= screens.Count)
        {
            currentScreenIndex = 0;
        }
        LoadCurrentScreen();
        UnloadScreen(oldIndex);
    }

    public void PreviousScreen()
    {
        int oldIndex = currentScreenIndex;
        currentScreenIndex--;
        if (currentScreenIndex < 0)
        {
            currentScreenIndex = screens.Count - 1;
        }
        LoadCurrentScreen();
        UnloadScreen(oldIndex);
    }

    public void LoadCurrentScreen()
    {
        LoadScreen(screens[currentScreenIndex].sceneName, screens[currentScreenIndex].backgroundColor);
    }
    
    public void LoadScreen(string name, Color color)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        background.color = color;

        AstronautManager.Instance.gameObject.GetComponent<MovableObject>().ResetPosition();

    }

    public void UnloadScreen(int index)
    {
        try
        {
            SceneManager.UnloadSceneAsync(screens[index].sceneName);
        }
        catch { }

    }
}
