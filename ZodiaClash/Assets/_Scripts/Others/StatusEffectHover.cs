using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHover : MonoBehaviour
{
    [SerializeField] private GameObject details;

    private void OnMouseEnter()
    {
        details.SetActive(true);
    }

    private void OnMouseExit()
    {
        details.SetActive(false);
    }
}
