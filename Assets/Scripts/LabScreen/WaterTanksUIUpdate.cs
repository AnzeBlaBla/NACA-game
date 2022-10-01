using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterTanksUIUpdate : MonoBehaviour
{
  [SerializeField] private GameObject unfilteredWaterTank;
  [SerializeField] private GameObject filteredWaterTank;

  void Start()
  {
    StartCoroutine(UpdateUI());
  }

  private IEnumerator UpdateUI()
  {
    while (true)
    {
      if (unfilteredWaterTank != null)
      {
        UnityEngine.UI.Slider slider = unfilteredWaterTank.GetComponent<UnityEngine.UI.Slider>();
        slider.value = StorageManager.Instance.data.unfilteredWater;
      }

      if (filteredWaterTank != null)
      {
        UnityEngine.UI.Slider slider = filteredWaterTank.GetComponent<UnityEngine.UI.Slider>();
        slider.value = StorageManager.Instance.data.filteredWater;
      }


      yield return new WaitForSeconds(1f);
    }
  }
}
