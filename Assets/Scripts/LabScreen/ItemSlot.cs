using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
  [SerializeField] private Texture[] textures;
  [SerializeField] private int growTime = 30;

  public void OnDrop(PointerEventData eventData)
  {
    int slotIndex = gameObject.transform.GetSiblingIndex();

    if (StorageManager.Instance.data.gardenSlots[slotIndex].saveTime == 0)
    {
      StorageManager.Instance.data.gardenSlots[slotIndex] = new StorageManager.GardenSlot()
      {
        index = slotIndex,
        saveTime = Epoch.Current(),
        growTime = growTime
      };

      StorageManager.Instance.SaveStats();

      GameObject seed = gameObject.transform.GetChild(0).gameObject;
      seed.GetComponent<UnityEngine.UI.RawImage>().texture = textures[0];
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    int slotIndex = gameObject.transform.GetSiblingIndex();

    if (StorageManager.Instance.data.gardenSlots[slotIndex].saveTime != 0)
    {
      if (Epoch.SecondsElapsed(StorageManager.Instance.data.gardenSlots[slotIndex].saveTime) >= StorageManager.Instance.data.gardenSlots[slotIndex].growTime)
      {
        StorageManager.Instance.data.gardenSlots[slotIndex] = new StorageManager.GardenSlot()
        {
          index = slotIndex,
          saveTime = 0,
          growTime = 0
        };

        StorageManager.Instance.SaveStats();

        GameObject seed = gameObject.transform.GetChild(0).gameObject;
        seed.GetComponent<UnityEngine.UI.RawImage>().texture = null;

        StorageManager.Instance.data.foodAmount += 1;
      }
    }
  }
}

