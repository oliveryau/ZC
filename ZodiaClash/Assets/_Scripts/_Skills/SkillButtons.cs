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
    private TextMeshProUGUI skillDescription;
    private GameObject[] players;
    private GameObject currentPlayer;

    private Image skillImage;

    private void Start()
    {
        battleManager = FindObjectOfType<BattleManager>();
        skillDescription = skillPanel.GetComponentInChildren<TextMeshProUGUI>();
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

                    skillDescription.text = "<color=#00FFFF>" + catSkill1.gameObject.name + "</color>\n\n" +
                        "Deals damage to a Single Enemy and inflicts Bleed (Max " + bleed.bleedLimit + ") for " + catSkill1.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill2Btn")
                {
                    B_AoeBleed catSkill2 = FindObjectOfType<B_AoeBleed>();

                    skillDescription.text = "<color=#00FFFF>" + catSkill2.gameObject.name + "</color>\n\n" +
                        "Deals damage to All Enemies and inflicts Bleed (Max " + bleed.bleedLimit + ") for " + catSkill2.bleedTurns + " turns.";
                }
                else if (gameObject.name == "CatSkill3Btn")
                {
                    C_AoeActivateBleed catSkill3 = FindObjectOfType<C_AoeActivateBleed>();

                    skillDescription.text = "<color=#00FFFF>" + catSkill3.gameObject.name + "</color>\n\n" +
                        "Deals damage to All Enemies and Consumes All Bleed Stacks. " +
                        "(If an enemy does not have any bleed stacks, only deals damage to the enemy.)";
                }

                break;

            case "Yangsheng":

                if (gameObject.name == "GoatSkill1Btn")
                {
                    Defense defense = FindObjectOfType<Defense>();
                    A_AttackDefBreak goatSkill1 = FindObjectOfType<A_AttackDefBreak>();

                    skillDescription.text = "<color=#00FFFF>" + goatSkill1.gameObject.name + "</color>\n\n" +
                        "Deals damage to a Single Enemy and inflicts Shatter (Max " + defense.shatterLimit + ") for " + goatSkill1.shatterTurns + " turns.";
                }
                else if (gameObject.name == "GoatSkill2Btn")
                {
                    Enrage enrage = FindObjectOfType<Enrage>();
                    B_AttackBuff goatSkill2 = FindObjectOfType<B_AttackBuff>();

                    skillDescription.text = "<color=#00FFFF>" + goatSkill2.gameObject.name + "</color>\n\n" +
                        "Allows An Ally (Except Yourself) to take action immediately and grants Enrage (Max " + enrage.enrageLimit + ") for " + goatSkill2.enrageTurns + " turns.";
                }
                else if (gameObject.name == "GoatSkill3Btn")
                {
                    C_SingleHeal goatSkill3 = FindObjectOfType<C_SingleHeal>();

                    skillDescription.text = "<color=#00FFFF>" + goatSkill3.gameObject.name + "</color>\n\n" +
                        "Heals and Removes All Negative Effects from An Ally.";
                }

                break;

            case "Leishou":

                if (gameObject.name == "OxSkill1Btn")
                {
                    A_SingleLifesteal oxSkill1 = FindObjectOfType<A_SingleLifesteal>();

                    skillDescription.text = "<color=#00FFFF>" + oxSkill1.gameObject.name + "</color>\n\n" +
                        "Deals damage to a Single Enemy and heals partially based on the damage dealt.";
                }
                else if (gameObject.name == "OxSkill2Btn")
                {
                    Stun stun = FindObjectOfType<Stun>();
                    B_SingleStun oxSkill2 = FindObjectOfType<B_SingleStun>();

                    skillDescription.text = "<color=#00FFFF>" + oxSkill2.gameObject.name + "</color>\n\n" +
                        "Deals damage to a Single Enemy and inflicts Stun (Max " + stun.stunLimit + ") for " + oxSkill2.stunTurns + " turns.";
                }
                else if (gameObject.name == "OxSkill3Btn")
                {
                    Taunt taunt = FindObjectOfType<Taunt>();
                    Defense defense = FindObjectOfType<Defense>();
                    C_AoeTaunt oxSkill3 = FindObjectOfType<C_AoeTaunt>();

                    skillDescription.text = "<color=#00FFFF>" + oxSkill3.gameObject.name + "</color>\n\n" +
                        "Deals damage to All Enemies and inflicts Taunt (Max " + taunt.tauntLimit + ") on Enemies (Max 2 Enemies) for " + oxSkill3.tauntTurns + " turns. " +
                        "Grants Armor (Max " + defense.armorLimit + ") on Yourself for " + oxSkill3.armorTurns + " turns.";
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
