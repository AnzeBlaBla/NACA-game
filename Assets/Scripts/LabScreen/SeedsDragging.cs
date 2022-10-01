using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeedsDragging : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  [SerializeField] private Canvas canvas;
  private RectTransform reactTransform;
  private CanvasGroup canvasGroup;

  private GameObject item;

  public void OnBeginDrag(PointerEventData eventData)
  {
    item = Instantiate(gameObject, transform.parent);
    item.transform.SetAsLastSibling();
    reactTransform = item.GetComponent<RectTransform>();
    canvasGroup = item.GetComponent<CanvasGroup>();

    canvasGroup.blocksRaycasts = false;
    canvasGroup.alpha = 0.6f;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    canvasGroup.blocksRaycasts = true;
    canvasGroup.alpha = 1f;
    Destroy(item);
  }

  public void OnDrag(PointerEventData eventData)
  {
    reactTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
  }

}
