using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusText : MonoBehaviour
{
    private float destroyTime;

    private void Start()
    {
        destroyTime = 0.7f;

        Destroy(gameObject, destroyTime); //destroy after a certain time
    }
}
