using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject skillPanel;

    private BattleManager battleManager;
    private TextMeshProUGUI skillTitle;
    private TextMeshProUGUI skillDescription;
    private GameObject[] players;
    private GameObject currentPlayer;

    private Image skillImage;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        skillTitle = skillPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        skillDescription = skillPanel.transform.Find("Description").GetComponent<TextMeshProUGUI>();
        skillImage = GetComponent<Image>();

        if (battleManager.battleState == BattleState.PLAYERTURN)
        {
            string chosenSkill = gameObject.name;
            gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(chosenSkill));

            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                if (player.name == battleManager.activePlayer)
                {
                    currentPlayer = player;
                    break;
                }
            }
        }
    }

    private void AttachCallback(string btn)
    {
        ResetSkillColor();

        switch (currentPlayer.name)
        {
            case "Guiying":

                if (btn.CompareTo("CatSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill1");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("CatSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill2");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("CatSkill3Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill3");
                    skillImage.color = Color.green;
                }

                break;

            case "Yangsheng":

                if (btn.CompareTo("GoatSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill1");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("GoatSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill2");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("GoatSkill3Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill3");
                    skillImage.color = Color.green;
                }

                break;

            case "Leishou":

                if (btn.CompareTo("OxSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill1");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("OxSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill2");
                    skillImage.color = Color.green;
                }
                else if (btn.CompareTo("OxSkill3Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill3");
                    skillImage.color = Color.green;
                }

                break;

            default:

                Debug.LogError("No current player found, BUG");
                break;
        }
    }

    public void ResetSkillColor()
    {
        SkillButtons[] allSkills = FindObjectsOfType<SkillButtons>();
        foreach (SkillButtons skill in allSkills)
        {
            skill.skillImage.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillDescription.text = null;

        switch (currentPlayer.name)
        {
            case "Guiying":

                Bleed catBleed = FindObjectOfType<Bleed>();
                if (gameObject.name == "CatSkill1Btn")
                {
                    A_SingleBleed catSkill1 = FindObjectOfType<A_SingleBleed>();

                    skillTitle.text = catSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and inflicts <color=red>Bleed</color> equal to " + catBleed.bleedPercent + "% of Guiying's attack for " + catSkill1.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill2Btn")
                {
                    B_AoeBleed catSkill2 = FindObjectOfType<B_AoeBleed>();

                    skillTitle.text = catSkill2.gameObject.name;
                    skillDescription.text = "Deals damage to all enemies and inflicts <color=red>Bleed</color> equal to " + catBleed.bleedPercent + "% of Guiying's attack for " + catSkill2.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill3Btn")
                {
                    C_AoeActivateBleed catSkill3 = FindObjectOfType<C_AoeActivateBleed>();

                    skillTitle.text = catSkill3.gameObject.name;
                    skillDescription.text = "Deals damage to all enemies, then if an enemy already has <color=red>Bleed</color>, all <color=red>Bleed</color> on that enemy will be activated and removed.";
                }

                break;

            case "Yangsheng":

                if (gameObject.name == "GoatSkill1Btn")
                {
                    Bleed goatBleed = FindObjectOfType<Bleed>();
                    A_AttackBleed goatSkill1 = FindObjectOfType<A_AttackBleed>();

                    skillTitle.text = goatSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and has a " + (goatSkill1.bleedRate * 100) + "% chance to inflict <color=red>Bleed</color> equal to " + goatBleed.bleedPercent + "% of Yangsheng's attack for " + goatSkill1.bleedTurns + " turn.";
                }
                else if (gameObject.name == "GoatSkill2Btn")
                {
                    Enrage enrage = FindObjectOfType<Enrage>();
                    B_AttackBuff goatSkill2 = FindObjectOfType<B_AttackBuff>();

                    skillTitle.text = goatSkill2.gameObject.name;
                    skillDescription.text = "Allows an ally (except Yangsheng) to take action immediately and grants <color=green>Enrage</color> equal to " + goatSkill2.skillBuffPercent + "% of the ally's attack for " + goatSkill2.enrageTurns + " turn.";
                }
                else if (gameObject.name == "GoatSkill3Btn")
                {
                    C_SingleHeal goatSkill3 = FindObjectOfType<C_SingleHeal>();

                    skillTitle.text = goatSkill3.gameObject.name;
                    skillDescription.text = "<color=green>Cleanse</color> and heals an ally equal to " + goatSkill3.skillBuffPercent + "% of Yangsheng's maximum health.";
                }

                break;

            case "Leishou":

                if (gameObject.name == "OxSkill1Btn")
                {
                    Defense defense = FindObjectOfType<Defense>();
                    A_SingleDefBreak oxSkill1 = FindObjectOfType<A_SingleDefBreak>();

                    skillTitle.text = oxSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and has a " + (oxSkill1.shatterRate * 100) + "% chance to inflict <color=red>Shatter</color> equal to " + oxSkill1.shatterPercent + "% of the enemy's defense for " + oxSkill1.shatterTurns + " turns.";
                }
                else if (gameObject.name == "OxSkill2Btn")
                {
                    Stun stun = FindObjectOfType<Stun>();
                    B_SingleStun oxSkill2 = FindObjectOfType<B_SingleStun>();

                    skillTitle.text = oxSkill2.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and inflicts <color=green>Stun</color> for " + oxSkill2.stunTurns + " turns.";
                }
                else if (gameObject.name == "OxSkill3Btn")
                {
                    Taunt taunt = FindObjectOfType<Taunt>();
                    Defense defense = FindObjectOfType<Defense>();
                    C_AoeTaunt oxSkill3 = FindObjectOfType<C_AoeTaunt>();

                    skillTitle.text = oxSkill3.gameObject.name;
                    skillDescription.text = "Inflicts <color=red>Taunt</color> on all enemies for " + oxSkill3.tauntTurns + " turn. Grants <color=green>Armor</color> equal to " + oxSkill3.armorPercent + "% of Leishou's defense on Leishou for " + oxSkill3.armorTurns + " turn.";
                }

                break;

            default:

                Debug.LogError("No current player found, BUG");
                break;
        }

        skillPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        skillPanel.SetActive(false);

        skillDescription.text = null;
    }
}
