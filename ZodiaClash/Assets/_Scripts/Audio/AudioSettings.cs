using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetEffectsVolume();
        }
    }

    public void SetMusicVolume()
    {
        AudioManager.Instance.MusicVolume(musicSlider.value);
    }

    public void SetEffectsVolume()
    {
        AudioManager.Instance.EffectsVolume(effectsSlider.value);
    }
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        effectsSlider.value = PlayerPrefs.GetFloat("effectsVolume");

        SetMusicVolume();
        SetEffectsVolume();
    }
}
