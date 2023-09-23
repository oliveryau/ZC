using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;

    private void Start()
    {
        StartCoroutine(FadeInPanel());
    }

    public IEnumerator FadeInPanel()
    {
        fadeInPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fadeInPanel.SetActive(false);
    }

    public IEnumerator FadeOutPanel()
    {
        fadeOutPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fadeOutPanel.SetActive(false);
    }
}
