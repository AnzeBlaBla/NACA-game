using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenHouseUIUpdate : MonoBehaviour
{
  [SerializeField] private GameObject gardenSlotsUI;
  [SerializeField] private Texture[] textures;

  private void Start()
  {
    StorageManager.Instance.onUpdate += UpdateUI;
  }

  private void UpdateUI()
  {
    for (int i = 0; i < StorageManager.Instance.data.gardenSlots.Count; i++)
    {
      StorageManager.GardenSlot slot = StorageManager.Instance.data.gardenSlots[i];

      if (slot.saveTime != 0)
      {
        int timePassed = Epoch.SecondsElapsed(slot.saveTime);
        int timeLeft = slot.growTime - timePassed;

        if (timeLeft <= 0)
        {
          StorageManager.Instance.data.gardenSlots[i] = new StorageManager.GardenSlot();
        }
        else
        {
          GameObject seed = gardenSlotsUI.transform.GetChild(i).GetChild(0).gameObject;



          int textureIndex = (int)Mathf.Floor((float)timePassed / (float)slot.growTime * (float)textures.Length);

          Debug.Log("textureIndex: " + textureIndex);
          Debug.Log("timePassed: " + timePassed);
          Debug.Log("timeLeft " + timeLeft);
          Debug.Log("slot.growTime " + slot.growTime);
          Debug.Log("slot.saveTime " + slot.saveTime);

          seed.GetComponent<UnityEngine.UI.RawImage>().texture = textures[textureIndex];
        }
      }
    }
  }
}
