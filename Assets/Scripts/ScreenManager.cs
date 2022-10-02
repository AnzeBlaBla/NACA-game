using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
  public GameObject nextButton;
  public GameObject backButton;

  public List<ScreenItem> screens;
  public int currentScreenIndex = 0;

  public Image background;

  [SerializeField] private GameObject sceneSwitchSound;

  public void Start()
  {
    // Unload all screens
    for (int i = 0; i < screens.Count; i++)
    {
      UnloadScreen(i);
    }

    // Load the first screen
    LoadCurrentScreen();
    this.UnloadScreenWithName("OutsideAirLockScreen");
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
    sceneSwitchSound.GetComponent<AudioSource>().Play();
  }

  public void LoadScreen(string name, Color color)
  {
    SceneManager.LoadScene(name, LoadSceneMode.Additive);
    background.color = color;

    AstronautManager.Instance.gameObject.GetComponent<MovableObject>().ResetPosition();

  }

  public void UnloadScreen(int index)
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
