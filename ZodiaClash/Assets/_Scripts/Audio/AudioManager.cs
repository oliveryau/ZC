using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sounds")]
    public AudioSource musicSource;
    public AudioSource effectsSource;
    public Sound[] musicSounds;
    public Sound[] effectsSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayEffectsOneShot(string name) //Play whole sound effect
    {
        Sound s = Array.Find(effectsSounds, x => x.name == name);

        effectsSource.clip = s.clip;
        effectsSource.PlayOneShot(s.clip);
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void EffectsVolume(float volume)
    {
        effectsSource.volume = volume;
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }
}
