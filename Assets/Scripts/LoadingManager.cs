using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingManager : MonoBehaviour
{
    public string mainSceneName;

    string thisSceneName;

    private void Awake()
    {
        thisSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainSceneName);
        while (!operation.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(thisSceneName);
    }
}
