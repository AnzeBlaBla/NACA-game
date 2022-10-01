using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
  [SerializeField] private Texture[] textures;
  [SerializeField] private int growTime = 30;

  private int GetCurrentIndex()
  {
    return transform.GetSiblingIndex();
  }


  private void SaveGardenSlots(bool isPlanting)
  {
    StorageManager.Instance.SaveStats();

    GameObject seed = gameObject.transform.GetChild(0).gameObject;
    seed.GetComponent<UnityEngine.UI.RawImage>().texture = isPlanting ? textures[0] : null;

    GameObject timeLeft = gameObject.transform.GetChild(2).gameObject;
    timeLeft.GetComponent<TextMeshProUGUI>().text = isPlanting ? Epoch.SecondsToDisplay(growTime) : "";
  }

  private void PutGardenSlot(int slotIndex, int saveTime)
  {
    StorageManager.Instance.data.gardenSlots[slotIndex] = new StorageManager.GardenSlot()
    {
      index = slotIndex,
      saveTime = saveTime,
      growTime = growTime
    };
  }

  public void OnDrop(PointerEventData eventData)
  {
    int slotIndex = GetCurrentIndex();

    if (StorageManager.Instance.data.gardenSlots[slotIndex].saveTime == 0)
    {
      PutGardenSlot(slotIndex, Epoch.Current());

      SaveGardenSlots(true);
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    int slotIndex = GetCurrentIndex();

    if (StorageManager.Instance.data.gardenSlots[slotIndex].saveTime != 0)
    {
      if (Epoch.SecondsElapsed(StorageManager.Instance.data.gardenSlots[slotIndex].saveTime) >= StorageManager.Instance.data.gardenSlots[slotIndex].growTime)
      {
        PutGardenSlot(slotIndex, 0);

        SaveGardenSlots(false);

        StorageManager.Instance.data.foodAmount += 1;
      }
    }
  }
}

