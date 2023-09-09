using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtons : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject[] players;
    private GameObject currentPlayer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager.state == BattleState.PLAYERTURN)
        {
            string chosenSkill = gameObject.name;
            gameObject.GetComponent<Button>().onClick.AddListener(() => AttachCallback(chosenSkill));

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

            default:

                Debug.Log("No current player found, BUG");
                break;
        }
    }
}
