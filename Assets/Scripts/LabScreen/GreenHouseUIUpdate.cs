using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GreenHouseUIUpdate : MonoBehaviour
{
  [SerializeField] private GameObject gardenSlotsUI;
  [SerializeField] private Texture[] textures;

  private void Start()
  {

    // update UI
    for (int i = 0; i < StorageManager.Instance.data.gardenSlots.Count; i++)
    {
      StorageManager.GardenSlot slot = StorageManager.Instance.data.gardenSlots[i];
      GameObject slotUI = gardenSlotsUI.transform.GetChild(i).gameObject;
      GameObject seed = slotUI.transform.GetChild(0).gameObject;

      if (slot.saveTime != 0)
      {
        int timePassed = Epoch.Current() - slot.saveTime;
        int timeLeft = slot.growTime - timePassed;

        int textureIndex = CalculateTextureIndex(timePassed, slot.growTime);

        if (timeLeft <= 0)
        {
          seed.GetComponent<UnityEngine.UI.RawImage>().texture = textures[textureIndex];
        }

        GameObject timeLeftUI = slotUI.transform.GetChild(2).gameObject;
        timeLeftUI.GetComponent<TextMeshProUGUI>().text = timeLeft <= 0 ? "Ready" : Epoch.SecondsToDisplay(timeLeft);
      }
    }

    StorageManager.Instance.onUpdate += UpdateUI;
  }

  private void UpdateUI()
  {
    if (gardenSlotsUI != null)
    {

      for (int i = 0; i < StorageManager.Instance.data.gardenSlots.Count; i++)
      {
        StorageManager.GardenSlot slot = StorageManager.Instance.data.gardenSlots[i];

        if (slot.saveTime != 0)
        {
          int timePassed = Epoch.SecondsElapsed(slot.saveTime);
          int timeLeft = slot.growTime - timePassed;

          if (timeLeft >= 0)
          {
            GameObject seed = gardenSlotsUI.transform.GetChild(i).GetChild(0).gameObject;

            int textureIndex = CalculateTextureIndex(timePassed, slot.growTime);
            seed.GetComponent<UnityEngine.UI.RawImage>().texture = textures[textureIndex];
          }

          GameObject timeLeftUI = gardenSlotsUI.transform.GetChild(i).GetChild(2).gameObject;
          timeLeftUI.GetComponent<TextMeshProUGUI>().text = timeLeft <= 0 ? "Ready" : Epoch.SecondsToDisplay(timeLeft);
        }
      }
    }
  }

  private int CalculateTextureIndex(int timePassed, int growTime)
  {
    float percent = (float)timePassed / (float)growTime;

    if (percent >= 1f)
    {
      return 2;
    }
    else if (percent >= 0.5f)
    {
      return 1;
    }
    else
    {
      return 0;
    }
  }
}
