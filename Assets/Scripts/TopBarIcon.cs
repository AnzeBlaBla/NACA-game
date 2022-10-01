using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBarIcon : MonoBehaviour
{
    public Gradient barColors;
    public string statName;

    public Image fillImage;
    // Start is called before the first frame update
    void Start()
    {
        AstronautManager.Instance.onUpdate += UpdateIcon;

        UpdateIcon();
    }

    void UpdateIcon()
    {
        float statValue = (float)AstronautManager.Instance.data.GetType().GetField(statName).GetValue(AstronautManager.Instance.data);

        fillImage.color = barColors.Evaluate(statValue / 100f);

        fillImage.fillAmount = statValue / 100f;

    }

}
