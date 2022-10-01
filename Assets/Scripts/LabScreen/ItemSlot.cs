using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
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
}

