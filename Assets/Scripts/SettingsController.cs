using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public static string musicVolumeKey = "MusicVolume";
    public static string sfxVolumeKey = "SFXVolume";

    public AudioMixer mixer;

    public bool musicEnabled = true;
    public bool SFXEnabled = true;

    public Button settingsButton;

    public GameObject settingsContainer;

    public Button musicButton;
    public Button SFXButton;

    public Sprite musicOn;
    public Sprite musicOff;

    public Sprite SFXOn;
    public Sprite SFXOff;


    public void Start()
    {
        if (PlayerPrefs.GetInt(musicVolumeKey, 1) == 1)
        {
            musicEnabled = true;
            musicButton.image.sprite = musicOn;
            mixer.SetFloat(musicVolumeKey, 0);
        }
        else
        {
            musicEnabled = false;
            musicButton.image.sprite = musicOff;
            mixer.SetFloat(musicVolumeKey, -80);
        }

        if (PlayerPrefs.GetInt(sfxVolumeKey, 1) == 1)
        {
            SFXEnabled = true;
            SFXButton.image.sprite = SFXOn;
            mixer.SetFloat(sfxVolumeKey, 0);
        }
        else
        {
            SFXEnabled = false;
            SFXButton.image.sprite = SFXOff;
            mixer.SetFloat(sfxVolumeKey, -80);
        }
    }

    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        if (musicEnabled)
        {
            musicButton.image.sprite = musicOn;
            PlayerPrefs.SetInt(musicVolumeKey, 1);
            mixer.SetFloat(musicVolumeKey, 0);
        }
        else
        {
            musicButton.image.sprite = musicOff;
            PlayerPrefs.SetInt(musicVolumeKey, 0);
            mixer.SetFloat(musicVolumeKey, -80);
        }
    }

    public void ToggleSFX()
    {
        SFXEnabled = !SFXEnabled;
        if (SFXEnabled)
        {
            SFXButton.image.sprite = SFXOn;
            PlayerPrefs.SetInt(sfxVolumeKey, 1);
            mixer.SetFloat(sfxVolumeKey, 0);
        }
        else
        {
            SFXButton.image.sprite = SFXOff;
            PlayerPrefs.SetInt(sfxVolumeKey, 0);
            mixer.SetFloat(sfxVolumeKey, -80);
        }
    }

    public void ToggleSettings()
    {
        settingsContainer.SetActive(!settingsContainer.activeSelf);
    }
}
