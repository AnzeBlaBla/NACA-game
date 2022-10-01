using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomScreenManager : MonoBehaviour
{
    public GameObject curtain;

    IEnumerator closeCurtain()
    {
        Vector2 position = curtain.GetComponent<RectTransform>().anchoredPosition;
 
        while (position.x < 0)
        {
            position.x += Screen.width / 20;
            curtain.GetComponent<RectTransform>().anchoredPosition = position;
            yield return new WaitForSeconds(0.1f);
        }
        position.x = 0;
        curtain.GetComponent<RectTransform>().anchoredPosition = position;
    }

    IEnumerator openCurtain()
    {
        Vector2 position = curtain.GetComponent<RectTransform>().anchoredPosition;

        while (position.x > -Screen.width)
        {
            position.x -= Screen.width / 20;
            curtain.GetComponent<RectTransform>().anchoredPosition = position;
            yield return new WaitForSeconds(0.1f);
        }
        position.x = -Screen.width + 10;
        curtain.GetComponent<RectTransform>().anchoredPosition = position;
    }

    IEnumerator closeAndOpenCurtain()
    {
        yield return StartCoroutine(closeCurtain());
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(openCurtain());
    }

    public void GoToTheBathroom()
    {
        StartCoroutine(closeAndOpenCurtain());
    }
}