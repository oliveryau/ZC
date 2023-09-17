using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime;
    private Vector3 offset;
    private Vector3 randomIntensity;

    private void Start()
    {
        destroyTime = 0.7f;
        offset = new Vector3(0, 1, 0);
        randomIntensity = new Vector3(0.2f, 0, 0);

        Destroy(gameObject, destroyTime); //destroy after a certain time

        transform.localPosition += offset; //offset from character transform

        transform.localPosition += new Vector3(Random.Range(-randomIntensity.x, randomIntensity.x),
            Random.Range(-randomIntensity.y, randomIntensity.y),
            Random.Range(-randomIntensity.z, randomIntensity.z)); //randomise text
    }
}
