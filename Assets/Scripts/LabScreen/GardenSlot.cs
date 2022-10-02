using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GardenSlot : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public static int growTime = 300;
    [SerializeField] private Texture[] textures;
    [SerializeField] private GameObject plantSound;
    [SerializeField] private GameObject harvestSound;

    private int GetCurrentIndex()
    {
        return transform.GetSiblingIndex();
    }


    private void SaveGardenSlots(bool isPlanting)
    {
        StorageManager.Instance.SaveData();

        GameObject seed = gameObject.transform.GetChild(0).gameObject;
        seed.GetComponent<UnityEngine.UI.RawImage>().texture = isPlanting ? textures[0] : textures[textures.Length - 1];

        GameObject timeLeft = gameObject.transform.GetChild(2).gameObject;
        timeLeft.GetComponent<TextMeshProUGUI>().text = isPlanting ? Epoch.SecondsToDisplay(growTime) : "";
    }

    private void PutGardenSlot(int slotIndex, int saveTime)
    {
        StorageManager.Instance.data.gardenSlots[slotIndex] = new StorageManager.GardenSlotStruct()
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

            plantSound.GetComponent<AudioSource>().Play();
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

                StorageManager.Instance.SaveData();

                harvestSound.GetComponent<AudioSource>().Play();
            }
        }
    }
}

