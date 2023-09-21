using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInfo : MonoBehaviour
{
    [SerializeField] private GameObject effectsInfo;

    public void ShowEffectsInfo()
    {
        effectsInfo.SetActive(true);

        Time.timeScale = 0f;
    }

    public void HideEffectsInfo()
    {
        effectsInfo.SetActive(false);

        Time.timeScale = 1f;
    }
}
