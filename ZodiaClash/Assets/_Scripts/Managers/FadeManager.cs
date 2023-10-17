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

    public void SpawnFadeOutPanel()
    {
        Instantiate(fadeOutPanel, transform.position, Quaternion.identity, this.gameObject.transform);
    }
}
