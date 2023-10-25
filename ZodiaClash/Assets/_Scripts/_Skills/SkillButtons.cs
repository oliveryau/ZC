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

                Bleed bleed = FindObjectOfType<Bleed>();

                if (gameObject.name == "CatSkill1Btn")
                {
                    A_SingleBleed catSkill1 = FindObjectOfType<A_SingleBleed>();

                    skillTitle.text = catSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and inflicts <color=red>Bleed</color> (Max " + bleed.bleedLimit + " turns) for " + catSkill1.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill2Btn")
                {
                    B_AoeBleed catSkill2 = FindObjectOfType<B_AoeBleed>();

                    skillTitle.text = catSkill2.gameObject.name;
                    skillDescription.text = "Deals damage to all enemies and inflicts <color=red>Bleed</color> (Max " + bleed.bleedLimit + " turns) for " + catSkill2.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill3Btn")
                {
                    C_AoeActivateBleed catSkill3 = FindObjectOfType<C_AoeActivateBleed>();

                    skillTitle.text = catSkill3.gameObject.name;
                    skillDescription.text = "Deals damage to all enemies. If an enemy already has <color=red>Bleed</color>, all <color=red>Bleed</color> on that enemy is consumed and reset immediately.";
                }

                break;

            case "Yangsheng":

                if (gameObject.name == "GoatSkill1Btn")
                {
                    Defense defense = FindObjectOfType<Defense>();
                    A_AttackDefBreak goatSkill1 = FindObjectOfType<A_AttackDefBreak>();

                    skillTitle.text = goatSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and inflicts <color=red>Shatter</color> (Max " + defense.shatterLimit + " turns) for " + goatSkill1.shatterTurns + " turns.";
                }
                else if (gameObject.name == "GoatSkill2Btn")
                {
                    Enrage enrage = FindObjectOfType<Enrage>();
                    B_AttackBuff goatSkill2 = FindObjectOfType<B_AttackBuff>();

                    skillTitle.text = goatSkill2.gameObject.name;
                    skillDescription.text = "Allows an ally (except Yangsheng) to take action immediately and grants <color=green>Enrage</color> for " + goatSkill2.enrageTurns + " turns.";
                }
                else if (gameObject.name == "GoatSkill3Btn")
                {
                    C_SingleHeal goatSkill3 = FindObjectOfType<C_SingleHeal>();

                    skillTitle.text = goatSkill3.gameObject.name;
                    skillDescription.text = "Heals an ally and grants <color=green>Cleanse</color>.";
                }

                break;

            case "Leishou":

                if (gameObject.name == "OxSkill1Btn")
                {
                    A_SingleLifesteal oxSkill1 = FindObjectOfType<A_SingleLifesteal>();

                    skillTitle.text = oxSkill1.gameObject.name;
                    skillDescription.text = "Deals damage to a single enemy and heals partially based on the damage dealt.";
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
                    skillDescription.text = "Inflicts <color=red>Taunt</color> on all enemies for " + oxSkill3.tauntTurns + " turns. Grants <color=green>Armor</color> on Leishou for " + oxSkill3.armorTurns + " turns.";
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
