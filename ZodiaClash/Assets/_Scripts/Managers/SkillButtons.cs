using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using TMPro;

public class SkillButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject skillPanel;

    private GameManager gameManager;
    private TextMeshProUGUI skillDescription;
    private GameObject[] players;
    private GameObject currentPlayer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        skillDescription = skillPanel.GetComponentInChildren<TextMeshProUGUI>();

        if (skillDescription == null)
        {
            Debug.Log("Hello");
        }

        if (gameManager.state == BattleState.PLAYERTURN)
        {
            string chosenSkill = gameObject.name;
            Button button = gameObject.GetComponent<Button>();

            button.onClick.AddListener(() => AttachCallback(chosenSkill));

            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (player.name == gameManager.activePlayer)
                {
                    currentPlayer = player;
                    break;
                }
            }
        }
    }

    private void AttachCallback(string btn)
    {
        switch (currentPlayer.name)
        {
            case "Cat":

                if (btn.CompareTo("Skill1Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("Skill2Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("Skill3Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill3");
                }

                break;

            case "Goat":

                if (btn.CompareTo("Skill1Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("Skill2Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("Skill3Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill3");
                }

                break;

            case "Ox":

                if (btn.CompareTo("Skill1Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("Skill2Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("Skill3Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill3");
                }

                break;

            default:

                Debug.LogError("No current player found, BUG");
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        skillDescription.text = null;

        switch (currentPlayer.name)
        {
            case "Cat":

                if (gameObject.name == "Skill1Btn")
                {
                    skillDescription.text = "<color=green>" + "Scarlet Claw" + "</color>\n\n" +
                        "Deals damage to a" + "<color=orange>" + " Single Target" + "</color>\n\n" +
                        "<color=orange>" + "Bleeds" + "</color>" + " the target";
                }
                else if (gameObject.name == "Skill2Btn")
                {
                    skillDescription.text = "<color=green>" + "Dancing Blades" + "</color>\n\n" +
                        "Deals damage to" + "<color=orange>" + " All Enemy Targets" + "</color>\n\n" +
                        "<color=orange>" + "Bleeds" + "</color>" + " all targets";
                }
                else if (gameObject.name == "Skill3Btn")
                {
                    skillDescription.text = "<color=green>" + "Bloodstorm Eclipse" + "</color>\n\n" +
                        "Deals damage to" + "<color=orange>" + " All Enemy Targets" + "</color>" + " by" + "<color=orange>" + " consuming all stacks of bleed\n\n" +
                        "<color=orange>" + "Bleeds" + "</color>" + " all targets";
                }

                break;

            case "Goat":

                if (gameObject.name == "Skill1Btn")
                {
                    skillDescription.text = "<color=green>" + "Yin-Yang Sunder" + "</color>\n\n" +
                        "Deals damage to a" + "<color=orange>" + " Single Target" + "</color>\n" +
                        "<color=orange>" + "Lowers Defense" + "</color>" + " of the target";
                }
                else if (gameObject.name == "Skill2Btn")
                {
                    skillDescription.text = "<color=green>" + "Ancestral Blessing" + "</color>\n\n" +
                        "Increases the attack of an" + "<color=orange>" + " Ally Single Target" + "</color>";
                }
                else if (gameObject.name == "Skill3Btn")
                {
                    skillDescription.text = "<color=green>" + "Celestial Renewal" + "</color>\n\n" +
                        "Heals an" + "<color=orange>" + " Ally Single Target" + "</color>\n\n" +
                        "<color=orange>" + "Dispels All Negative Effects" + "</color>" + " of the target";
                }

                break;

            case "Ox":

                if (gameObject.name == "Skill1Btn")
                {
                    skillDescription.text = "<color=green>" + "Mountain's Wrath" + "</color>\n\n" +
                        "Deals damage to" + "<color=orange>" + " All Enemy Targets" + "</color>";
                }
                else if (gameObject.name == "Skill2Btn")
                {
                    skillDescription.text = "<color=green>" + "Unyielding Thunder" + "</color>\n\n" +
                        "Stuns a" + "<color=orange>" + " Single Target" + "</color>";
                }
                else if (gameObject.name == "Skill3Btn")
                {
                    skillDescription.text = "<color=green>" + "Ox King's Challenge" + "</color>\n\n" +
                        "Taunts a" + "<color=orange>" + " Single Target" + "</color>";
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
