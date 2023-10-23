using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Button musicToggle;
    public Button effectsToggle;

    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("ambienceVolume") || PlayerPrefs.HasKey("effectsVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetEffectsVolume();
        }
    }

    public void ToggleMusicVolume()
    {
        AudioManager.Instance.ToggleMusic();
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void ToggleEffectsVolume()
    {
        AudioManager.Instance.ToggleEffects();
    }

    public void SetEffectsVolume()
    {
        AudioManager.Instance.EffectsVolume(effectsSlider.value);
    }

    public void LoadVolume()
    {
        //musicToggle = PlayerPrefs.

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsSlider.value = PlayerPrefs.GetFloat("ambienceVolume");
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");

        SetMusicVolume();
        SetEffectsVolume();
    }
}
