using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomScreenManager : MonoBehaviour
{
  public GameObject curtain;

  private bool isInAnimation = false;

  [SerializeField] private GameObject toiletFlushSound;


  public void Start()
  {
    Vector2 position = curtain.GetComponent<RectTransform>().anchoredPosition;
    position.x = -Screen.width + 20;
    curtain.GetComponent<RectTransform>().anchoredPosition = position;
  }

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
    position.x = -Screen.width + 20;
    curtain.GetComponent<RectTransform>().anchoredPosition = position;
  }

  public IEnumerator closeAndOpenCurtain()
  {
    if (isInAnimation)
      yield break;
    isInAnimation = true;
    yield return StartCoroutine(closeCurtain());
    yield return new WaitForSeconds(3);

    toiletFlushSound.GetComponent<AudioSource>().Play();

    yield return StartCoroutine(openCurtain());
    isInAnimation = false;
  }
}
