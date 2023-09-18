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

                if (btn.CompareTo("CatSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("CatSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("CatSkill3Btn") == 0)
                {
                    currentPlayer.GetComponent<CatAction>().SelectSkill("skill3");
                }

                break;

            case "Goat":

                if (btn.CompareTo("GoatSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("GoatSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("GoatSkill3Btn") == 0)
                {
                    currentPlayer.GetComponent<GoatAction>().SelectSkill("skill3");
                }

                break;

            case "Ox":

                if (btn.CompareTo("OxSkill1Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill1");
                }
                else if (btn.CompareTo("OxSkill2Btn") == 0)
                {
                    currentPlayer.GetComponent<OxAction>().SelectSkill("skill2");
                }
                else if (btn.CompareTo("OxSkill3Btn") == 0)
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

                if (gameObject.name == "CatSkill1Btn")
                {
                    skillDescription.text = "Scarlet Claw\n\n" +
                        "<color=orange>" + "Single Target" + "</color>\n" +
                        "<color=red>" + "Bleeds (Max 5 Stacks)" + "</color>";
                }
                else if (gameObject.name == "CatSkill2Btn")
                {
                    skillDescription.text = "Dancing Blades\n" +
                        "<color=yellow>" + "x2 Chi" + "</color>\n\n" +
                        "<color=orange>" + "All Enemy Targets" + "</color>\n" +
                        "<color=red>" + "Bleeds (Max 5 Stacks)" + "</color>";
                }
                else if (gameObject.name == "CatSkill3Btn")
                {
                    skillDescription.text = "Bloodstorm Eclipse\n" +
                        "<color=yellow>" + "x3 Chi" + "</color>\n\n" +
                        "<color=orange>" + "All Enemy Targets" + "</color>\n" +
                        "<color=red>" + "Consumes All Bleed Stacks" + "</color>";
                }

                break;

            case "Goat":

                if (gameObject.name == "GoatSkill1Btn")
                {
                    skillDescription.text = "Yin-Yang Sunder\n\n" +
                        "<color=orange>" + "Single Target" + "</color>\n" +
                        "<color=red>" + "Lowers Defense Stat for 2 turns" + "</color>";
                }
                else if (gameObject.name == "GoatSkill2Btn")
                {
                    skillDescription.text = "Ancestral Blessing\n" +
                        "<color=yellow>" + "x2 Chi" + "</color>\n\n" +
                        "<color=orange>" + "Ally Single Target" + "</color>\n" +
                        "<color=green>" + "Increase Attack Stat for 2 turns" + "</color>";
                }
                else if (gameObject.name == "GoatSkill3Btn")
                {
                    skillDescription.text = "Celestial Renewal\n" +
                        "<color=yellow>" + "x3 Chi" + "</color>\n\n" +
                        "<color=orange>" + "Ally Single Target" + "</color>\n" +
                        "<color=green>" + "Dispels All Negative Effects" + "</color>";
                }

                break;

            case "Ox":

                if (gameObject.name == "OxSkill1Btn")
                {
                    skillDescription.text = "Mountain's Wrath\n\n" +
                        "<color=orange>" + "All Enemy Targets" + "</color>";
                }
                else if (gameObject.name == "OxSkill2Btn")
                {
                    skillDescription.text = "Unyielding Thunder\n" +
                        "<color=yellow>" + "x3 Chi" + "</color>\n\n" +
                        "<color=orange>" + "Single Target" + "</color>\n" +
                        "<color=red>" + "Stun" + "</color>";
                }
                else if (gameObject.name == "OxSkill3Btn")
                {
                    skillDescription.text = "Ox King's Challenge\n" +
                        "<color=yellow>" + "x3 Chi" + "</color>\n\n" +
                        "<color=orange>" + "Single Target" + "</color>\n" +
                        "<color=red>" + "Taunt" + "</color>";
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
