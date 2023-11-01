using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class _StatusEffectHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hover;

    private TextMeshProUGUI hoverText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverText = hover.GetComponentInChildren<TextMeshProUGUI>();

        switch (gameObject.name)
        {
            case "Bleed(Clone)":
                hoverText.text = "Bleed";
                break;

            case "Shatter(Clone)":
                hoverText.text = "Shatter";
                break;

            case "Stun(Clone)":
                hoverText.text = "Stun";
                break;

            case "Taunt(Clone)":
                hoverText.text = "Taunt";
                break;

            case "Enrage(Clone)":
                hoverText.text = "Enrage";
                break;

            case "Armor(Clone)":
                hoverText.text = "Armor";
                break;

            case "Rage(Clone)":
                hoverText.text = "Berserk";
                break;

            default:
                break;
        }

        hover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hover.SetActive(false);

        hoverText.text = null;
    }
}
