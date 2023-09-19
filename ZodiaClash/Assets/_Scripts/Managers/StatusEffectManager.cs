using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffectManager : MonoBehaviour
{
    [Header("Effects Panel")]
    [SerializeField] private Transform effectsPanel;

    [Header("Debuffs")]
    [SerializeField] private GameObject bleedIcon;
    [SerializeField] private GameObject defBreakIcon;
    [SerializeField] private GameObject stunIcon;
    [SerializeField] private GameObject tauntIcon;

    [Header("Buffs")]
    [SerializeField] private GameObject atkBuffIcon;

    public void SpawnEffectsBar(CharacterStats target, int count, string effect)
    {
        effectsPanel = target.statusEffectPanel;

        switch (effect)
        {
            //debuffs
            case "bleed":

                bool bleedExist = false;

                _Bleed bleed = FindObjectOfType<_Bleed>();

                for (int i = 0; i < target.statusEffectPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI bleedText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Bleed")) //if bleedIcon exists
                    {
                        bleedExist = true;

                        if (int.TryParse(bleedText.text, out int value)) //convert int to string
                        {
                            value += count; //add the int and bleedCount

                            if (value > bleed.bleedLimit) //if it exceeds the bleedLimit
                            {
                                value = bleed.bleedLimit; //set to bleedLimit
                            }

                            bleedText.text = value.ToString(); //set the number of the text
                        }
                        break;
                    }
                }

                if (!bleedExist) //instantiate normally if no bleedIcon
                {
                    TextMeshProUGUI newBleedText = bleedIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newBleedText.text = count.ToString();
                    Instantiate(bleedIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "defBreak":

                bool defBreakExist = false;

                for (int i = 0; i < target.statusEffectPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI defBreakText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Def Break")) //if defBreakIcon exists
                    {
                        defBreakExist = true;

                        if (int.TryParse(defBreakText.text, out int value)) //convert int to string
                        {
                            value = count; //set to defBreakCount

                            defBreakText.text = value.ToString(); //set the number of the text
                        }
                        break;
                    }
                }

                if (!defBreakExist)
                {
                    TextMeshProUGUI newDefBreakText = defBreakIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newDefBreakText.text = count.ToString();
                    Instantiate(defBreakIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "stun":
                TextMeshProUGUI stunText = stunIcon.GetComponentInChildren<TextMeshProUGUI>();
                stunText.text = count.ToString();
                Instantiate(stunIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                break;

            case "taunt":
                TextMeshProUGUI tauntText = tauntIcon.GetComponentInChildren<TextMeshProUGUI>();
                tauntText.text = count.ToString();
                Instantiate(tauntIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                break;

            //buffs
            case "atkBuff":
                TextMeshProUGUI atkBuffText = atkBuffIcon.GetComponentInChildren<TextMeshProUGUI>();
                atkBuffText.text = count.ToString();
                Instantiate(atkBuffIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                break;

            default:
                break;
        }

        effectsPanel = null;
    }

    public void EarlyUpdateEffectsBar(CharacterStats character, string effect)
    {
        switch (effect)
        {
            case "bleed":

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;    
                    TextMeshProUGUI bleedText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Bleed"))
                    {
                        bleedText.text = character.bleedStack.ToString();

                        if (character.bleedStack <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "defBreak":

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI defBreakText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Def Break"))
                    {
                        defBreakText.text = character.defBreakCounter.ToString();

                        if (character.defBreakCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            default:
                break;
        }
    }
}
