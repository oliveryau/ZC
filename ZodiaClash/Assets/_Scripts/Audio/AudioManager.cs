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
    public AudioSource ambienceSource;
    public AudioSource effectsSource;
    public Sound[] musicSounds;
    public Sound[] ambienceSounds;
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

    public void PlayAmbienceMusic(string name)
    {
        Sound s = Array.Find(ambienceSounds, x => x.name == name);

        ambienceSource.clip = s.clip;
        ambienceSource.Play();
    }

    public void StopAmbienceMusic()
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
        ambienceSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("ambienceVolume", volume);
    }

    public void EffectsVolume(float volume)
    {
        effectsSource.volume = volume;
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }

    public IEnumerator AudioFade(bool fadeIn, float duration)
    {
        if (fadeIn)
        {
            float time = 0f;
            float startVol = 0f;
            float targetVol = musicSource.volume;
            while (time < duration)
            {
                time += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVol, targetVol, time / duration);
                yield return null;
            }

            yield break;
        }
        else if (!fadeIn)
        {
            float time = 0f;
            float startVol = musicSource.volume;
            float targetVol = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVol, targetVol, time / duration);
                yield return null;
            }

            yield break;
        }
    }

    public void MusicFadeIn(bool fadeIn)
    {
        if (fadeIn)
        {
            StartCoroutine(AudioFade(true, 1f));
        }
        else if (!fadeIn)
        {
            StartCoroutine(AudioFade(false, 1f));
        }
    }
}
