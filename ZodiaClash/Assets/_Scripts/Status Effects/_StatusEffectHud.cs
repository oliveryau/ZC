using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.GraphicsBuffer;

public class _StatusEffectHud : MonoBehaviour
{
    private Transform effectsPanel;

    [Header("Debuffs")]
    [SerializeField] private GameObject bleedIcon;
    [SerializeField] private GameObject shatterIcon;
    [SerializeField] private GameObject stunIcon;
    [SerializeField] private GameObject tauntIcon;

    [Header("Buffs")]
    [SerializeField] private GameObject atkBuffIcon;
    [SerializeField] private GameObject armorIcon;
    [SerializeField] private GameObject rageGoatBuffIcon;

    public void SpawnEffectsBar(CharacterStats target, int count, string effect)
    {
        effectsPanel = target.statusEffectPanel.transform;

        switch (effect)
        {
            //debuffs
            case "bleed":

                bool bleedExist = false;
                Bleed bleed = FindObjectOfType<Bleed>();

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = target.statusEffectPanel.transform.GetChild(i).gameObject;
                    TextMeshProUGUI bleedText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Bleed")) //if bleedIcon exists
                    {
                        bleedExist = true;

                        if (int.TryParse(bleedText.text, out int value)) //convert int to string
                        {
                            value += count; //add the int and bleedCount

                            if (value > bleed.bleedLimit) //if it exceeds bleedLimit
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

            case "shatter":

                bool shatterExist = false;
                Defense shatter = FindObjectOfType<Defense>();

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI shatterText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Shatter")) //if shatterIcon exists
                    {
                        shatterExist = true;

                        if (int.TryParse(shatterText.text, out int value)) //convert int to string
                        {
                            value += count; //add the int and shatterCount

                            if (value > shatter.shatterLimit) //if it exceeds shatterLimit
                            {
                                value = shatter.shatterLimit; //set to shatterLimit
                            }

                            shatterText.text = value.ToString(); //set the number of the text
                        }
                        break;
                    }
                }

                if (!shatterExist) //instantiate normally if no defBreakIcon
                {
                    TextMeshProUGUI newshatterText = shatterIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newshatterText.text = count.ToString();
                    Instantiate(shatterIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "stun":

                bool stunExist = false;

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
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

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
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
            case "enrage":

                bool atkBuffExist = false;

                Enrage enrage = FindObjectOfType<Enrage>();

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI enrageText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Enrage"))
                    {
                        atkBuffExist = true;

                        if (int.TryParse(enrageText.text, out int value))
                        {
                            value += count; //add the int and enrageCount

                            if (value > enrage.enrageLimit) //if it exceeds enrageLimit
                            {
                                value = enrage.enrageLimit; //set to enrageLimit
                            }

                            enrageText.text = value.ToString();
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

            case "armor":

                bool armorExist = false;

                Defense armor = FindObjectOfType<Defense>();

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI armorText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Armor"))
                    {
                        armorExist = true;

                        if (int.TryParse(armorText.text, out int value))
                        {
                            value += count; //add the int and enrageCount

                            if (value > armor.armorLimit) //if it exceeds enrageLimit
                            {
                                value = armor.armorLimit; //set to enrageLimit
                            }

                            armorText.text = value.ToString();
                        }
                        break;
                    }
                }

                if (!armorExist)
                {
                    TextMeshProUGUI newArmorText = armorIcon.GetComponentInChildren<TextMeshProUGUI>();
                    newArmorText.text = count.ToString();
                    Instantiate(armorIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            case "rageGoat":

                bool rageGoatBuffExist = false;

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;

                    if (status.CompareTag("RageGoat"))
                    {
                        rageGoatBuffExist = true;

                        break;
                    }
                }

                if (!rageGoatBuffExist)
                {
                    Instantiate(rageGoatBuffIcon, effectsPanel.position, Quaternion.identity, effectsPanel);
                }

                break;

            default:
                break;
        }

        effectsPanel = null;
    }

    public void UpdateEffectsBar(CharacterStats character, string effect)
    {
        effectsPanel = character.statusEffectPanel.transform;

        switch (effect)
        {
            case "bleed":

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;    
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

            case "shatter":

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI defBreakText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Shatter"))
                    {
                        defBreakText.text = character.shatterCounter.ToString();

                        if (character.shatterCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "stun":

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
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

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
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

            case "enrage":

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI atkBuffText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Enrage"))
                    {
                        atkBuffText.text = character.enrageCounter.ToString();

                        if (character.enrageCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "armor":

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;
                    TextMeshProUGUI armorText = status.GetComponentInChildren<TextMeshProUGUI>();

                    if (status.CompareTag("Armor"))
                    {
                        armorText.text = character.armorCounter.ToString();

                        if (character.armorCounter <= 0)
                        {
                            Destroy(status);
                        }
                    }
                }

                break;

            case "cleanse": //add all buff tags

                for (int i = 0; i < effectsPanel.childCount; i++)
                {
                    GameObject status = effectsPanel.GetChild(i).gameObject;

                    if (status.CompareTag("Bleed") || status.CompareTag("Shatter") || status.CompareTag("Stun") || status.CompareTag("Taunt"))
                    {
                        Destroy(status);
                    }
                }

                break;

            default: 
                break;
        }
    }
}
