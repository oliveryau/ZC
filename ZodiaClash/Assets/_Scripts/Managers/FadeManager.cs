using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour
{
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;

    private void Start()
    {
        Destroy(Instantiate(fadeInPanel, transform.position, Quaternion.identity, this.gameObject.transform), 1f);
    }

    public IEnumerator FadeOutPanel()
    {
        fadeOutPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fadeOutPanel.SetActive(false);
    }
}
