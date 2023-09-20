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
                            value = count; //defBreakCount will always be defBreak limit

                            defBreakText.text = value.ToString(); //set the number of the text
                        }
                        break;
                    }
                }

                if (!defBreakExist) //instantiate normally if no defBreakIcon
                {
                    TextMeshProUGUI newDefBreakText = defBreakIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newDefBreakText.text = count.ToString();
                    Instantiate(defBreakIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "stun":

                bool stunExist = false;

                for (int i = 0; i < target.statusEffectPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI stunText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Stun")) //if stunIcon exists
                    {
                        stunExist = true;

                        if (int.TryParse(stunText.text, out int value)) //convert int to string
                        {
                            value = count; //stunCounter will always be stun limit

                            stunText.text = value.ToString();
                        }
                        break;
                    }
                }

                if (!stunExist) //instantiate normally if no stunIcon
                {
                    TextMeshProUGUI newStunText = stunIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newStunText.text = count.ToString();
                    Instantiate(stunIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "taunt":

                bool tauntExist = false;

                for (int i = 0; i < target.statusEffectPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI tauntText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Stun"))
                    {
                        tauntExist = true;

                        if (int.TryParse(tauntText.text, out int value))
                        {
                            value = count; //tauntCounter will always be taunt limit

                            tauntText.text = value.ToString();
                        }
                        break;
                    }
                }

                if (!tauntExist) //instantiate normally if no tauntIcon
                {
                    TextMeshProUGUI newTauntText = tauntIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newTauntText.text = count.ToString();
                    Instantiate(tauntIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            //buffs
            case "atkBuff":

                bool atkBuffExist = false;

                for (int i = 0; i < target.statusEffectPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI atkBuffText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Atk Buff"))
                    {
                        atkBuffExist = true;

                        if (int.TryParse(atkBuffText.text, out int value))
                        {
                            value = count; //attackkBuffCounter will always be atkBuff limit

                            atkBuffText.text = value.ToString();
                        }
                        break;
                    }
                }

                if (!atkBuffExist)
                {
                    TextMeshProUGUI newAtkBuffText = atkBuffIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newAtkBuffText.text = count.ToString();
                    Instantiate(atkBuffIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            default:
                break;
        }

        effectsPanel = null;
    }

    public void UpdateEffectsBar(CharacterStats character, string effect)
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

            case "stun":

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI stunText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Stun"))
                    {
                        stunText.text = character.stunCounter.ToString();

                        if (character.stunCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "taunt":

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI tauntText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Taunt"))
                    {
                        tauntText.text = character.tauntCounter.ToString();

                        if (character.tauntCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "atkBuff":

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;
                    TextMeshProUGUI atkBuffText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Atk Buff"))
                    {
                        atkBuffText.text = character.attackBuffCounter.ToString();

                        if (character.attackBuffCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            default:

                for (int i = 0; i < character.statusEffectPanel.childCount; i++)
                {
                    GameObject status = character.statusEffectPanel.GetChild(i).gameObject;

                    if (!status.CompareTag("Atk Buff"))
                    {
                        Destroy(status);
                    }
                }

                break;
        }
    }
}
