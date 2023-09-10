using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime;
    private Vector3 offset;

    private void Start()
    {
        destroyTime = 0.5f;
        offset = new Vector3(0, 1, 0);

        Destroy(gameObject, destroyTime);
        transform.localPosition += offset;
    }
}
