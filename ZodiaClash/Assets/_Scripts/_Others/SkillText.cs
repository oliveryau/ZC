using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillText : MonoBehaviour
{
    private TextMeshPro skillDescriptionText;

    private void Start()
    {
        skillDescriptionText = GetComponent<TextMeshPro>();
    }

    public void SetSkillDescription(string description)
    {
        skillDescriptionText.text = description;
    }

    public void ClearSkillDescription()
    {
        skillDescriptionText.text = null;
    }
}
